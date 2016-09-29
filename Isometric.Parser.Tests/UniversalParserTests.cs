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
            var command = value.ToString();

            // act
            int result;
            var success = command.TryParse(out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual(result, value);
        }


        [TestMethod]
        public void GetValueString_TryParse_Integer_Equality()
        {
            // arrange
            const int value = 123;
            var command = value.GetValueString();

            // act
            int result;
            var success = command.TryParse(out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual(result, value);
        }


        [TestMethod]
        public void GetValueString_TryParse_Resources_Equality()
        {
            // arrange
            var value = new Resources(corn: 400, gold: 200, people: 20, meat: 20);

            // act
            Resources result;
            var success = value.GetValueString().TryParse(out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual(result, value);
        }
    }
}