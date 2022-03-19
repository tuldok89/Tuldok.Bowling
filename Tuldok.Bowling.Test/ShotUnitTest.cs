using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuldok.Bowling.Service.Exceptions;

namespace Tuldok.Bowling.Test
{
    [TestClass]
    public class ShotUnitTest : BaseUnitTest
    {
        public ShotUnitTest() : base()
        {

        }

        [TestMethod]
        public async Task TestShotCreation()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);
            var shot = await BowlingService.CreateShot(frame.Id, 5);
            var shot2 = await BowlingService.CreateShot(frame.Id, 5);

            Assert.IsNotNull(shot);
            Assert.AreEqual(shot.FallenPins, 5);
            Assert.AreEqual(shot.SequenceNumber, 1);
            Assert.AreEqual(shot2.SequenceNumber, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(PinFallsExceededException))]
        public async Task TestTooManyPins()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);
            var shot = await BowlingService.CreateShot(frame.Id, 6);
            var shot2 = await BowlingService.CreateShot(frame.Id, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityCountExceededException))]
        public async Task TestTooManyShots()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);

            for (int i = 0; i < 3; i++)
            {
                _ = await BowlingService.CreateShot(frame.Id, 5);
            }
        }

        [TestMethod]
        public async Task TestInvalidPinfalls()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);
            
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            {
                _ = await BowlingService.CreateShot(frame.Id, -1);
            });

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            {
                _ = await BowlingService.CreateShot(frame.Id, 11);
            });
        }

        [TestMethod]
        public async Task TestInvalidSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            {
                _ = await BowlingService.CreateShot(frame.Id, 1, 0);
            });

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            {
                _ = await BowlingService.CreateShot(frame.Id, 1, 3);
            });
        }

        [TestMethod]
        public async Task TestInvalidSequence10()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            {
                _ = await BowlingService.CreateShot(frame.Id, 1, 0);
            });

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () =>
            {
                _ = await BowlingService.CreateShot(frame.Id, 1, 4);
            });
        }

        [TestMethod]
        public async Task TestSpare10()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);

            _ = await BowlingService.CreateShot(frame.Id, 7);
            _ = await BowlingService.CreateShot(frame.Id, 3);
            _ = await BowlingService.CreateShot(frame.Id, 4);
        }

        [TestMethod]
        public async Task TestStrike10()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);

            _ = await BowlingService.CreateShot(frame.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityCountExceededException))]
        public async Task TestTooManyShots10()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 7);
            _ = await BowlingService.CreateShot(frame.Id, 2);
            _ = await BowlingService.CreateShot(frame.Id, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityCountExceededException))]
        public async Task TestTooManyShots10Spare()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 5);
            _ = await BowlingService.CreateShot(frame.Id, 5);
            _ = await BowlingService.CreateShot(frame.Id, 5);
            _ = await BowlingService.CreateShot(frame.Id, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityCountExceededException))]
        public async Task TestTooManyShots10Strike()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(PinFallsExceededException))]
        public async Task TestTooManyPins10()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);
            _ = await BowlingService.CreateShot(frame.Id, 7);
            _ = await BowlingService.CreateShot(frame.Id, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateSequenceException))]
        public async Task TestDuplicateSequence()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id, 10);

            _ = await BowlingService.CreateShot(frame.Id, 4, 1);
            _ = await BowlingService.CreateShot(frame.Id, 2, 1);
        }
    }

    
}
