using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Girvel.Graph;
using Isometric.Core.Modules;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Implementation.Modules.GameData.Exceptions;

namespace Isometric.Implementation.Modules.GameData
{
    [Serializable]
    public class GameDataManager
    {
        public static GameDataManager Instance { get; set; }

        public BuildingPattern[] Patterns { get; }



        /// <exception cref="InvalidGameDataException">Thrown when data in stream is invalid</exception>
        public GameDataManager(Stream stream)
        {
            GameDataContainer data;

            try
            {
                data = (GameDataContainer) new BinaryFormatter().Deserialize(stream);
            }
            catch (SerializationException ex)
            {
                throw new InvalidGameDataException("Game data can not be deserialized", ex);
            }

            Patterns = data.Patterns;
            BuildingGraph.Instance = data.BuildingGraph;

            foreach (var property in GameConstantAttribute.GetProperties())
            {
                if (!data.Constants.ContainsKey(property.Name))
                {
                    throw new InvalidGameDataException("Game data does not contain all game constants");
                }
                property.SetValue(null, data.Constants[property.Name]);
            }
        }

        public BuildingPattern GetBuildingPattern(string name) => Patterns.First(p => p.Name == name);




    }
}