using System.Linq;
using Isometric.CommonStructures;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.TickModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Isometric.CoreTests.Modules.PlayerModule
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Test_Tick_ShouldCallIndependentSubjects()
        {
            // arrange
            var player = new Player();
            var mock = new Mock<IIndependentChanging>();
            player.IndependentSubjects.Add(mock.Object);

            // act
            player.Tick();

            // assert
            mock.Verify(i => i.Tick(), Times.Once);
        }

        [TestMethod]
        public void Test_Tick_ShouldSummarizeResourceSubjects()
        {
            // arrange
            var player = new Player {CurrentResources = new Resources()};

            player.ResourceSubjects.Add(
                Mock.Of<IResourcesChanging>(
                    i => i.Tick() == new Resources(0, 20, 100, 0, 0, 0)));

            player.ResourceSubjects.Add(
                Mock.Of<IResourcesChanging>(
                    i => i.Tick() == new Resources(0, 80, 0, 1, 1, 1)));

            // act
            player.Tick();

            // assert
            Assert.AreEqual(new Resources(0, 100, 100, 1, 1, 1), player.CurrentResources);
        }

        [TestMethod]
        public void Test_AddOwnedBuilding_ShouldChangeLists()
        {
            // arrange
            var player = new Player();
            var building = new Building();

            // act
            player.AddOwnedBuilding(building);

            // assert
            Assert.IsTrue(
                player.GetOwnedBuildings().Contains(building),
               "player.GetOwnedBuildings().Contains(building)");

            Assert.IsTrue(
                player.IndependentSubjects.Contains(building),
               "player.IndependentSubjects.Contains(building)");

            Assert.IsTrue(
                player.ResourceSubjects.Contains(building),
               "player.ResourceBonusSubjects.Contains(building)");

            Assert.IsTrue(
                player.ResourceBonusSubjects.Contains(building),
               "player.ResourceBonusSubjects.Contains(building)");
        }
    }
}