using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Data.Entities;
using Tuldok.Bowling.Service.Interfaces;

namespace Tuldok.Bowling.Service
{
    public class ScoreService : IScoreService
    {
        private readonly IFrameService _frameService;
        private readonly IShotService _shotService;

        public ScoreService(IFrameService frameService, IShotService shotService)
        {
            _frameService = frameService;
            _shotService = shotService;
        }

        public async Task<int> GameScore(Guid gameGuid)
        {
            int totalScore = 0;
            var frames = (await _frameService.GetFrames(gameGuid));
            frames.Sort((x, y) =>
            {
                return x.SequenceNumber.CompareTo(y.SequenceNumber);
            });

            for (int i = 0; i < frames.Count; i++)
            {
                var frame = frames[i];

                if (i != 9)
                {
                    var frameDetails = await GetFrameDetails(frame.Id);

                    totalScore += frameDetails.Shots.Sum(x => x.FallenPins);

                    if (frameDetails.IsStrike)
                    {
                        var plus1idx = i + 1;

                        // break if it exceeds array bounds
                        if (plus1idx > frames.Count)
                        {
                            continue;
                        }

                        var framePlus1 = frames[i + 1];
                        var frameDetails1 = await GetFrameDetails(framePlus1.Id);
                        if (frameDetails1.Shots.Count >= 2)
                        {
                            totalScore += frameDetails1.Shots.Take(2).Sum(x => x.FallenPins);
                        }
                        else
                        {
                            var plus2idx = i + 2;

                            // break if it exceeds array bounds
                            if (plus2idx > frames.Count)
                            {
                                continue;
                            }

                            var framePlus2 = frames[plus2idx];
                            var frameDetails2 = await GetFrameDetails(framePlus2.Id);
                            totalScore += frameDetails1.Shots.First().FallenPins + frameDetails2.Shots.First().FallenPins;
                        }
                    }
                    else if (frameDetails.IsSpare)
                    {
                        var plus1idx = i + 1;

                        if (plus1idx > frames.Count)
                        {
                            continue;
                        }

                        var framePlus1 = frames[i + 1];
                        var frameDetails1 = await GetFrameDetails(framePlus1.Id);

                        totalScore += frameDetails1.Shots.First().FallenPins;
                    }
                }
                else // 10th game
                {
                    var frameDetails = await GetFrame10Details(frame.Id);
                    totalScore += frameDetails.Sum(x => x.FallenPins);
                }
            }

            return totalScore;
        }

        private async Task<(bool IsStrike, bool IsSpare, List<Shot> Shots)> GetFrameDetails(Guid frameId)
        {
            var shots = await _shotService.GetShots(frameId);

            shots.Sort((x, y) =>
            {
                return x.SequenceNumber.CompareTo(y.SequenceNumber);
            });

            var pinfalls = shots.Sum(x => x.FallenPins);

            var isStrike = (pinfalls == 10) && (shots.Count == 1);
            var isSpare = (pinfalls == 10) && (shots.Count == 2);

            return (isStrike, isSpare, shots);
        }

        private async Task<List<Shot>> GetFrame10Details(Guid frameId)
        {
            var shots = await _shotService.GetShots(frameId);

            shots.Sort((x, y) =>
            {
                return x.SequenceNumber.CompareTo(y.SequenceNumber);
            });

            return shots;
        }
    }
}
