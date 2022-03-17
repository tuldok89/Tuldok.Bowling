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
    public class BowlingService
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
            var game = new Game {
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

            if (!(frameNumber > 0 && frameNumber <= 10))
            {
                throw new ArgumentOutOfRangeException(nameof(frameNumber));
            }

            if (frameNumber == null)
            {
                var currentSequence = game.Frames.Select(x => x.FrameNumber);
                frameNumber = GetNextSequence(currentSequence, 10);
            }

            var frame = new Frame
            {
                Id = Guid.NewGuid(),
                Game = game,
                FrameNumber = frameNumber.Value
            };

            var rows = await _frameService.InsertFrame(frame);

            if (rows == 0)
            {
                throw new EntityNotCreatedException(nameof(Frame));
            }

            return frame;
        }

        public async Task<List<Frame>> GetAllFrames(Guid gameId) => await _frameService.GetFrames(gameId);

        public async Task<Frame> GetFrame(Guid frameId) => await _frameService.GetFrame(frameId);

        public async Task DeleteFrame(Guid frameId) => await _frameService.DeleteFrame(frameId);
        
        public async Task<Shot> CreateShot(Guid frameId, int pinFalls, int? shotNumber = null)
        {
            if (!(pinFalls > 0 && pinFalls <= 10))
            {
                throw new ArgumentOutOfRangeException(nameof(shotNumber));
            }

            var frame = await _frameService.GetFrame(frameId);

            frame.Shots = await _shotService.GetShots(frameId);

            if (frame.FrameNumber < 10)
            {
                if (frame.Shots.Count() == 2)
                {
                    throw new EntityCountExceededException(nameof(Shot), 2);
                }

                if (!(shotNumber > 0 && shotNumber <= 2))
                {
                    throw new ArgumentOutOfRangeException(nameof(shotNumber));
                }

                if (shotNumber == null)
                {
                    var currentSequence = frame.Shots.Select(x => x.ShotNumber);
                    shotNumber = GetNextSequence(currentSequence, 2);
                }

                var currentPinfalls = frame.Shots.Sum(x => x.FallenPins);
                if (currentPinfalls + pinFalls > 10)
                {
                    var remainingPins = 10 - currentPinfalls;

                    throw new PinFallsExceededException(remainingPins);
                }

                var shot = new Shot
                {
                    Id = Guid.NewGuid(),
                    FallenPins = pinFalls,
                    Frame = frame,
                    ShotNumber = shotNumber.Value
                };

                var rows = await _shotService.InsertShot(shot);

                if (rows == 0)
                {
                    throw new EntityNotCreatedException(nameof(Shot));
                }

                return shot;
            }
            else // 10th frame
            {
                if (!(shotNumber > 0 && shotNumber <= 3))
                {
                    throw new ArgumentOutOfRangeException(nameof(shotNumber));
                }
                
                if (shotNumber == null)
                {
                    var currentSequence = frame.Shots.Select(x => x.ShotNumber);
                    shotNumber = GetNextSequence(currentSequence, 3);
                }

                if (shotNumber == 3 && (frame.Shots?.First().FallenPins != 10 || frame.Shots?.Sum(x => x.FallenPins) != 10))
                {
                    throw new EntityCountExceededException(nameof(Shot), 2);
                }

                var fallenPins1 = frame.Shots?.First().FallenPins;

                if (shotNumber == 2 && (fallenPins1 != 10 && (fallenPins1 + pinFalls > 10)))
                {
                    fallenPins1 ??= 0;
                    var remainingPins = 10 - fallenPins1;
                    throw new PinFallsExceededException(remainingPins.Value);
                }

                var shot = new Shot
                {
                    FallenPins = pinFalls,
                    Frame = frame,
                    Id = Guid.NewGuid(),
                    ShotNumber = shotNumber.Value
                };

                var rows = await _shotService.InsertShot(shot);
                
                if (rows == 0)
                {
                    throw new EntityNotCreatedException(nameof(Shot));
                }

                return shot;
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
