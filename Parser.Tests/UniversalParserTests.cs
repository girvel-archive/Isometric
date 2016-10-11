using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Buildings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RandomExtensions;

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
            var success = str.TryParse(typeof(int), null, out result);

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
            var success = command.TryParse(typeof(int), null, out result);

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
            var success = value.GetValueString(typeof(Resources)).TryParse(typeof(Resources), null, out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual((Resources)result, value);
        }



        [TestMethod]
        public void String_TryParse_Resources_Equality()
        {
            // arrange
            const string str = "meat: 200 corn: 100";
            var expectedResult = new Resources(meat: 200, corn: 100);

            // act
            object result;
            var success = str.TryParse(typeof(Resources), null, out result);

            // assert
            Assert.IsTrue(success);
            Assert.AreEqual(expectedResult, (Resources) result);
        }



        //[TestMethod]
        //public void String_TryParse_RandomCollection_Equality()
        //{
        //    // arrange
        //    const string str = "Forest: 8 Rock: 1";
        //    var forest = Mock.Of<BuildingPattern>(p => p.Id == 0 && p.);
            
        //    var expectedResult = new[]
        //    {
        //        new RandomPair<BuildingPattern>(8, Buildin)
        //    };
        //}
    }
}