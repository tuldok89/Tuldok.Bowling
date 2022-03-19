using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Test
{
    [TestClass]
    public class ScoreUnitTest : BaseUnitTest
    {
        /// <summary>
        /// Reference: http://www.fryes4fun.com/Bowling/scoring.htm
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestScoring()
        {
            var game = await BowlingService.CreateGame("Test");
            
            var frame1 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame1.Id, 8);
            _ = BowlingService.CreateShot(frame1.Id, 2);

            var frame2 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame2.Id, 5);
            _ = BowlingService.CreateShot(frame2.Id, 4);

            var frame3 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame3.Id, 9);
            _ = BowlingService.CreateShot(frame3.Id, 0);

            var frame4 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame4.Id, 10);

            var frame5 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame5.Id, 10);

            var frame6 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame6.Id, 5);
            _ = BowlingService.CreateShot(frame6.Id, 5);

            var frame7 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame7.Id, 5);
            _ = BowlingService.CreateShot(frame7.Id, 3);

            var frame8 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame8.Id, 6);
            _ = BowlingService.CreateShot(frame8.Id, 3);

            var frame9 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame9.Id, 9);
            _ = BowlingService.CreateShot(frame9.Id, 1);

            var frame10 = await BowlingService.CreateFrame(game.Id);
            _ = BowlingService.CreateShot(frame10.Id, 9);
            _ = BowlingService.CreateShot(frame10.Id, 1);
            _ = BowlingService.CreateShot(frame10.Id, 10);

            var score = await ScoreService.GameScore(game.Id);

            Assert.AreEqual(149, score);
        }
    }
}
