using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Editor.Containers;
using Isometric.Implementation.Modules.GameData;
using Isometric.Implementation.Modules.GameData.Exceptions;

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
        
        /// <exception cref="InvalidGameDataException">Thrown when loading data is invalid</exception>
        public GameData(Stream stream)
        {
            GameDataContainer container;
            try
            {
                container = (GameDataContainer)new BinaryFormatter().Deserialize(stream);
            }
            catch (SerializationException ex)
            {
                throw new InvalidGameDataException("Game data can not be deserialized"); 
            }

            BuildingPatterns = new BuildingPatternCollection(container.Patterns);
            BuildingGraph = container.BuildingGraph;
            Constants = container.Constants.ToDictionary(
                pair => pair.Key, 
                pair => new ConstantContainer(pair.Value, pair.Value.GetType()));
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