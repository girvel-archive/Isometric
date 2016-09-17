using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Editor
{
    public class GameData
    {
        public static GameData Instance { get; set; }



        public List<BuildingPattern> BuildingPatterns { get; set; }

        public Graph<BuildingPattern> BuildingGraph { get; set; }



        public GameData()
        {
            BuildingPatterns = new List<BuildingPattern>();
            BuildingGraph = new Graph<BuildingPattern>();
        }
        
        public GameData(Stream stream)
        {
            var formatter = new BinaryFormatter();

            BuildingPatterns = ((BuildingPattern[]) formatter.Deserialize(stream)).ToList();
            BuildingGraph = (Graph<BuildingPattern>) formatter.Deserialize(stream);
        }



        public void SerializeData(Stream stream)
        {
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, BuildingPatterns.ToArray());
            formatter.Serialize(stream, BuildingGraph);
        }
    }
}