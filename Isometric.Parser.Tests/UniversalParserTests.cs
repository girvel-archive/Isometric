using Isometric.CommonStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Isometric.Parser.Tests
{
    [TestClass]
    public class UniversalParserTests
    {
        [TestMethod]
        public void ToString_TryParse_Integer_Equality()
        {
            // arrange
            const int value = 123;
            var str = value.ToString();

            // act
            object result;
            var success = str.TryParse(typeof(int), out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual((int) result, value);
        }



        [TestMethod]
        public void GetValueString_TryParse_Integer_Equality()
        {
            // arrange
            const int value = 123;
            var command = value.GetValueString(typeof(int));

            // act
            object result;
            var success = command.TryParse(typeof(int), out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual((int)result, value);
        }



        [TestMethod]
        public void GetValueString_TryParse_Resources_Equality()
        {
            // arrange
            var value = new Resources(corn: 400, gold: 200, people: 20, meat: 20);

            // act
            object result;
            var success = value.GetValueString(typeof(Resources)).TryParse(typeof(Resources), out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual((Resources)result, value);
        }



        [TestMethod]
        public void String_TryParse_Resources_Equality()
        {
            // arrange
            var value = "meat: 200 corn: 100";
            var necessaryResult = new Resources(meat: 200, corn: 100);

            // act
            object result;
            var success = value.TryParse(typeof(Resources), out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual((Resources) result, necessaryResult);
        }
    }
}