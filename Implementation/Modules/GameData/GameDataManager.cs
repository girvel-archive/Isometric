using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Implementation.Modules.GameData
{
    [Serializable]
    public class GameDataManager
    {
        public static GameDataManager Instance { get; set; }

        public BuildingPattern[] Patterns { get; }



        public GameDataManager(Stream stream)
        {
            var data = (GameData) new BinaryFormatter().Deserialize(stream);

            Patterns = data.Patterns;
            BuildingGraph.Instance = data.BuildingGraph;
        }

        public BuildingPattern GetPattern(string name) => Patterns.First(p => p.Name == name);
    }
}