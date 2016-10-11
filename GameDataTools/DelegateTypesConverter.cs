using System;
using System.Collections.Generic;
using Isometric.Core.Modules.WorldModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using RandomExtensions;

namespace Isometric.GameDataTools
{
    public class DelegateTypesConverter
    {
        private static DelegateTypesConverter _instance;
        public static DelegateTypesConverter Instance => _instance ?? (_instance = new DelegateTypesConverter());



        private DelegateTypesConverter()
        {
            DataByDelegate = new Dictionary<Type, Type>
            {
                [typeof(World.AreaGenerator)] = typeof(RandomCollection<BuildingPattern>),
                [typeof(World.VillageGenerator)] = typeof(World.DefaultBuilding[]),
            };
        }



        public Dictionary<Type, Type> DataByDelegate { get; }
    }
}
