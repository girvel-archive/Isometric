using System;
using Isometric.CommonStructures;
using Isometric.Core.Modules.PlayerModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Core.Modules.WorldModule.Land;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorNet;

namespace Isometric.Core.Tests.Modules.WorldModule.Land
{
    [TestClass]
    public class AreaTests
    {
        [TestMethod]
        public void IndexerSettingTest()
        {
#pragma warning disable 618

            var area = new Area(12);

            var b = new Building(new IntVector(0, 0), Player.Nature, area, new BuildingPattern());

#pragma warning restore 618

            area[new IntVector(0, 0)] = b;

            Assert.Equals(area[new IntVector(0, 0)], b);
        }
    }
}