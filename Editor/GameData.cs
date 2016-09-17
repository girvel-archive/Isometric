using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules.WorldModule.Buildings;
using SaveGameData = Isometric.Implementation.Modules.GameData.GameData;

namespace Isometric.Editor
{
    public class GameData
    {
        public static GameData Instance { get; set; }



        public BuildingPatternCollection BuildingPatterns { get; set; }

        public Graph<BuildingPattern> BuildingGraph { get; set; }



        public GameData()
        {
            BuildingPatterns = new BuildingPatternCollection();
            BuildingGraph = new Graph<BuildingPattern>();
        }
        
        public GameData(Stream stream)
        {
            var data = (SaveGameData)new BinaryFormatter().Deserialize(stream);

            BuildingPatterns = new BuildingPatternCollection(data.Patterns);
            BuildingGraph = data.BuildingGraph;
        }



        public void SerializeData(Stream stream)
        {
            new BinaryFormatter().Serialize(
                stream, 
                new SaveGameData(
                    BuildingPatterns.ToArray(), 
                    BuildingGraph));
        }
    }
}