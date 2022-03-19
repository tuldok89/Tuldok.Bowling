using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Tuldok.Bowling.Service.Exceptions;

namespace Tuldok.Bowling.Test
{
    [TestClass]
    public class BowlingUnitTest : BaseUnitTest
    {
        

        public BowlingUnitTest() : base()
        {

        }

        [TestMethod]
        public async Task TestGameCreation()
        {
            var game = await BowlingService.CreateGame("Test");

            Assert.IsNotNull(game);
            Assert.AreEqual(game.Name, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task TestNonExistentGame()
        {
            var guid = Guid.NewGuid();

            var game1 = await BowlingService.GetGame(guid);
        }



        [TestMethod]
        public async Task TestShotCreation()
        {
            var game = await BowlingService.CreateGame("Test");
            var frame = await BowlingService.CreateFrame(game.Id);
            
        }
    }
}