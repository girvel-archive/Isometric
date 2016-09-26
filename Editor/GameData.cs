using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Editor.Containers;
using Isometric.Implementation.Modules.GameData;
using Isometric.Implementation.Modules.GameData.Exceptions;
using RandomExtensions;

namespace Isometric.Editor
{
    public class GameData
    {
        public static GameData Instance { get; set; }



        public BuildingPatternCollection BuildingPatterns { get; set; }

        public Graph<BuildingPattern> BuildingGraph { get; set; }

        public Dictionary<string, ConstantContainer> Constants { get; set; }



        private static readonly Dictionary<Type, Type> DelegateTypes;



        static GameData()
        {
            DelegateTypes = new Dictionary<Type, Type>
            {
                [typeof(World.AreaGenerator)] = typeof(RandomCollection<BuildingPattern>),
                [typeof(World.VillageGenerator)] = typeof(World.DefaultBuilding[]),
            };
        }



        /// <summary>
        /// Creates empty game data
        /// </summary>
        public GameData()
        {
            BuildingPatterns = new BuildingPatternCollection();
            BuildingGraph = new Graph<BuildingPattern>();
            Constants = new Dictionary<string, ConstantContainer>();

            foreach (var property in GameConstantAttribute.GetProperties())
            {
                Type type;

                if (!DelegateTypes.TryGetValue(property.PropertyType, out type))
                {
                    type = property.PropertyType;
                }

                Constants[property.Name] = new ConstantContainer(null, type);
            }
        }

        /// <summary>
        /// Loads game data from stream
        /// </summary>
        /// <param name="stream">Stream with game data</param>
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
                throw new InvalidGameDataException("Game data can not be deserialized", ex);
            }

            BuildingPatterns = new BuildingPatternCollection(container.Patterns);
            BuildingGraph = container.BuildingGraph;

            var properties = GameConstantAttribute.GetProperties();
            if (!Constants.All(constant => properties.Any(property => property.Name == constant.Key)))
            {
                throw new InvalidGameDataException("Game data does not contain all constants");
            }

            Constants = container.Constants.ToDictionary(
                pair => pair.Key,
                pair => new ConstantContainer(pair.Value, GetEditableType(pair.Value)));
        }



        /// <summary>
        /// Serializes data to specified stream
        /// </summary>
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



        protected Type GetEditableType(object obj)
        {
            Type type = obj.GetType();

            return DelegateTypes.ContainsKey(type) ? DelegateTypes[type] : type;
        }
    }
}