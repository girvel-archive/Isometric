using Isometric.Core.Modules.PlayerModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Isometric.CoreTests.Modules.PlayerModule
{
    [TestClass]
    public class PlayersManagerTests
    {
        [TestMethod]
        public void Test_Tick_ShouldCallPlayersTicks()
        {
            // arrange
            var manager = new PlayersManager();
            var playerMock1 = new Mock<Player>();
            var playerMock2 = new Mock<Player>();
            manager.Players.Add(playerMock1.Object);
            manager.Players.Add(playerMock2.Object);

            // act
            manager.Tick();

            // assert
            playerMock1.Verify(p => p.Tick(), Times.Once, "playerMock1.Tick()");
            playerMock2.Verify(p => p.Tick(), Times.Once, "playerMock2.Tick()");
        }
    }
}