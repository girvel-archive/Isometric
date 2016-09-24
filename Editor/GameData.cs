using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Editor.Containers;
using Isometric.Implementation.Modules.GameData;

namespace Isometric.Editor
{
    public class GameData
    {
        public static GameData Instance { get; set; }



        public BuildingPatternCollection BuildingPatterns { get; set; }

        public Graph<BuildingPattern> BuildingGraph { get; set; }

        public Dictionary<string, ConstantContainer> Constants { get; set; } 



        public GameData()
        {
            BuildingPatterns = new BuildingPatternCollection();
            BuildingGraph = new Graph<BuildingPattern>();
            Constants = new Dictionary<string, ConstantContainer>();

            foreach (var property in GameConstantAttribute.GetProperties())
            {
                Constants[property.Name] = new ConstantContainer(null, property.PropertyType);
            }
        }
        
        public GameData(Stream stream)
        {
            var data = (GameDataContainer)new BinaryFormatter().Deserialize(stream);

            BuildingPatterns = new BuildingPatternCollection(data.Patterns);
            BuildingGraph = data.BuildingGraph;
        }



        public void SerializeData(Stream stream)
        {
            new BinaryFormatter().Serialize(
                stream, 
                new GameDataContainer(
                    BuildingPatterns.ToArray(), 
                    BuildingGraph,
                    Constants.ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value.Value)));
        }
    }
}