using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Tuldok.Bowling.Service.Exceptions;

namespace Tuldok.Bowling.Test
{
    [TestClass]
    public class FrameUnitTest : BaseUnitTest
    {
        [TestMethod]
        public async Task TestFrameCreation()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);

            Assert.IsNotNull(frame);
            Assert.AreEqual(frame.SequenceNumber, 1);
        }

        [TestMethod]
        public async Task TestFrameAutoSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame1 = await BowlingService.CreateFrame(game.Id);
            var frame2 = await BowlingService.CreateFrame(game.Id);
            var frame3 = await BowlingService.CreateFrame(game.Id);

            Assert.AreEqual(frame1.SequenceNumber, 1);
            Assert.AreEqual(frame2.SequenceNumber, 2);
            Assert.AreEqual(frame3.SequenceNumber, 3);
        }

        [TestMethod]
        public async Task TestFrameSkippedSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame3 = await BowlingService.CreateFrame(game.Id, 3);
            var frame1 = await BowlingService.CreateFrame(game.Id);
            var frame2 = await BowlingService.CreateFrame(game.Id);
            var frame4 = await BowlingService.CreateFrame(game.Id);

            Assert.AreEqual(frame1.SequenceNumber, 1);
            Assert.AreEqual(frame2.SequenceNumber, 2);
            Assert.AreEqual(frame3.SequenceNumber, 3);
            Assert.AreEqual(frame4.SequenceNumber, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityCountExceededException))]
        public async Task TestFrameCountExceeded()
        {
            var game = await BowlingService.CreateGame("Test");

            for (int i = 0; i < 11; i++)
            {
                _ = await BowlingService.CreateFrame(game.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestTooBigFrameSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 11);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestTooSmallFrameSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateSequenceException))]
        public async Task TestFrameDuplicateSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 1);
            var frame2 = await BowlingService.CreateFrame(game.Id, 1);
        }
    }
}
