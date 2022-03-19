using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Service.Exceptions;
using Tuldok.Bowling.Service.Interfaces;

namespace Tuldok.Bowling.Service
{
    public class BowlingService : IBowlingService
    {
        private readonly IGameService _gameService;
        private readonly IFrameService _frameService;
        private readonly IShotService _shotService;

        public BowlingService(IGameService gameService, IFrameService frameService, IShotService shotService)
        {
            _gameService = gameService;
            _frameService = frameService;
            _shotService = shotService;
        }

        public async Task<Game> CreateGame(string name)
        {
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            var rows = await _gameService.InsertGame(game);

            if (rows == 0) // game not created
            {
                throw new EntityNotCreatedException(nameof(Game));
            }

            return game;
        }

        public async Task UpdateGame(Game game)
        {
            var gameToUpdate = await GetGame(game.Id);

            gameToUpdate.Name = game.Name;

            var rows = await _gameService.UpdateGame(gameToUpdate);

            if (rows == 0)
            {
                throw new EntityNotUpdatedException(nameof(Game));
            }
        }

        public async Task<Game> GetGame(Guid id)
        {
            var game = await _gameService.GetGame(id);

            if (game == null)
            {
                throw new EntityNotFoundException(nameof(Game));
            }

            return game;
        }

        public async Task<List<Game>> GetAllGames() => await _gameService.GetGames();

        public async Task DeleteGame(Guid gameId) => await _gameService.DeleteGame(gameId);

        public async Task<Frame> CreateFrame(Guid gameId, int? frameNumber = null)
        {
            var game = await GetGame(gameId);
            game.Frames = await GetAllFrames(gameId);

            if (game.Frames.Count() == 10)
            {
                throw new EntityCountExceededException(nameof(Frame), 10);
            }

            if (frameNumber == null)
            {
                var currentSequence = game.Frames.Select(x => x.SequenceNumber);
                frameNumber = GetNextSequence(currentSequence, 10);
            }

            if (!(frameNumber > 0 && frameNumber <= 10))
            {
                throw new ArgumentOutOfRangeException(nameof(frameNumber));
            }

            var frame = new Frame
            {
                Id = Guid.NewGuid(),
                Game = game,
                SequenceNumber = frameNumber.Value
            };

            if (await _frameService.HasDuplicateSequence(game.Id, frame.Id, frameNumber.Value))
            {
                throw new DuplicateSequenceException(nameof(Frame));
            }

            var rows = await _frameService.InsertFrame(frame);

            if (rows == 0)
            {
                throw new EntityNotCreatedException(nameof(Frame));
            }

            return frame;
        }

        public async Task UpdateFrame(Frame frame)
        {
            var frameToUpdate = await GetFrame(frame.Id);

            // check for duplicate frame number
            if (frameToUpdate.SequenceNumber != frame.SequenceNumber)
            {
                if (await _frameService.HasDuplicateSequence(frameToUpdate.GameId, frameToUpdate.Id, frame.SequenceNumber))
                {
                    throw new DuplicateSequenceException(nameof(Frame));
                }
            }

            frameToUpdate.SequenceNumber = frame.SequenceNumber;

            var rows = await _frameService.UpdateFrame(frameToUpdate);

            if (rows == 0)
            {
                throw new EntityNotUpdatedException(nameof(Frame));
            }
        }

        public async Task<List<Frame>> GetAllFrames(Guid gameId) => await _frameService.GetFrames(gameId);

        public async Task<Frame> GetFrame(Guid frameId) => await _frameService.GetFrame(frameId);

        public async Task DeleteFrame(Guid frameId) => await _frameService.DeleteFrame(frameId);

        public async Task<Shot> CreateShot(Guid frameId, int pinFalls, int? sequenceNumber = null)
        {
            if (!(pinFalls >= 0 && pinFalls <= 10))
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceNumber));
            }

            var frame = await _frameService.GetFrame(frameId);

            frame.Shots = await _shotService.GetShots(frameId);

            if (frame.SequenceNumber < 10)
            {

                CheckShot(frame, pinFalls, ref sequenceNumber);

                var shot = new Shot
                {
                    Id = Guid.NewGuid(),
                    FallenPins = pinFalls,
                    Frame = frame,
                    SequenceNumber = sequenceNumber.Value
                };

                if (await _shotService.HasDuplicateSequence(frameId, shot.Id, shot.SequenceNumber))
                {
                    throw new DuplicateSequenceException(nameof(Shot));
                }

                var rows = await _shotService.InsertShot(shot);

                if (rows == 0)
                {
                    throw new EntityNotCreatedException(nameof(Shot));
                }

                return shot;
            }
            else // 10th frame
            {
                CheckShotFrame10(frame, pinFalls, ref sequenceNumber);

                var shot = new Shot
                {
                    FallenPins = pinFalls,
                    Frame = frame,
                    Id = Guid.NewGuid(),
                    SequenceNumber = sequenceNumber.Value
                };

                if (await _shotService.HasDuplicateSequence(frameId, shot.Id, shot.SequenceNumber))
                {
                    throw new DuplicateSequenceException(nameof(Shot));
                }

                var rows = await _shotService.InsertShot(shot);

                if (rows == 0)
                {
                    throw new EntityNotCreatedException(nameof(Shot));
                }

                return shot;
            }
        }

        private void CheckShot(Frame frame, int pinFalls, ref int? sequenceNumber)
        {
            if (frame.Shots.Count() == 2)
            {
                throw new EntityCountExceededException(nameof(Shot), 2);
            }

            if (sequenceNumber == null)
            {
                var currentSequence = frame.Shots.Select(x => x.SequenceNumber);
                sequenceNumber = GetNextSequence(currentSequence, 2);
            }

            if (!(sequenceNumber > 0 && sequenceNumber <= 2))
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceNumber));
            }

            var currentPinfalls = frame.Shots.Sum(x => x.FallenPins);
            if (currentPinfalls + pinFalls > 10)
            {
                var remainingPins = 10 - currentPinfalls;

                throw new PinFallsExceededException(remainingPins);
            }
        }

        private void CheckShotFrame10(Frame frame, int pinFalls, ref int? sequenceNumber)
        {
            if (frame.Shots.Count() == 3)
            {
                throw new EntityCountExceededException(nameof(Shot), 3);
            }

            if (sequenceNumber == null)
            {
                var currentSequence = frame.Shots.Select(x => x.SequenceNumber);
                sequenceNumber = GetNextSequence(currentSequence, 3);
            }

            if (!(sequenceNumber > 0 && sequenceNumber <= 3))
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceNumber));
            }

            var firstShotPins = frame.Shots?.FirstOrDefault()?.FallenPins;
            var sumTwoShots = frame.Shots?.Sum(x => x.FallenPins);

            // 1st shot isn't a strike, nor are the previous shots a spare
            if (sequenceNumber == 3 && firstShotPins != 10 && sumTwoShots != 10)
            {
                throw new EntityCountExceededException(nameof(Shot), 2);
            }

            // 2nd shot exceeds the total number of pins
            if (sequenceNumber == 2 && firstShotPins != 10 && firstShotPins + pinFalls > 10)
            {
                firstShotPins ??= 0;
                var remainingPins = 10 - firstShotPins;
                throw new PinFallsExceededException(remainingPins.Value);
            }
        }

        public async Task<List<Shot>> GetAllShots(Guid frameId) => await _shotService.GetShots(frameId);
        public async Task<Shot> GetShot(Guid shotId) => await _shotService.GetShot(shotId);
        public async Task DeleteShot(Guid shotId) => await _shotService.DeleteShot(shotId);

        private static int GetNextSequence(IEnumerable<int> numbers, int max)
        {
            var range = new HashSet<int>(Enumerable.Range(1, max));
            range.ExceptWith(numbers);

            return range.Min();
        }
    }
}
