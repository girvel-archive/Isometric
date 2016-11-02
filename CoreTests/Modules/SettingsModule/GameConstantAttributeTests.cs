using System.Linq;
using System.Reflection;
using Isometric.Core.Modules.SettingsModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Isometric.CoreTests.Modules.SettingsModule
{
    [TestClass]
    public class GameConstantAttributeTests
    {
        [TestMethod]
        public void Test_GetProperties_ShouldReturnAllConstants()
        {
            // arrange
            var publicConstantProperty =
                typeof (TestClass_GameElement).GetProperty(nameof(TestClass_GameElement.PublicConstant));

            var privateConstantProperty =
                typeof (TestClass_GameElement).GetProperty(
                    "PrivateConstant",
                    BindingFlags.NonPublic | BindingFlags.Static);

            // act
            var properties = GameConstantAttribute.GetProperties(typeof (TestClass_GameElement).Assembly);

            // assert
            Assert.AreEqual(2, properties.Length, "Number of returned properties is wrong");

            Assert.IsTrue(
                properties.Contains(publicConstantProperty),
                "Returned properties does not contain public constant");

            Assert.IsTrue(
                properties.Contains(privateConstantProperty),
                "Returned properties does not contain private constant");
        }

        

        private class TestClass_GameElement
        {
            [GameConstant]
            public static string PublicConstant { get; set; }

            [GameConstant]
            private static string PrivateConstant { get; set; }
        }
    }
}