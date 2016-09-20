using System.Collections.Generic;
using Isometric.Core.Modules.SettingsModule;

namespace Isometric.Implementation.Modules.DataImplementation
{
    public static class GameConstantsManager
    {
        private static readonly Dictionary<string, object> ConstantValues;

        static GameConstantsManager()
        {
            ConstantValues = new Dictionary<string, object>
            {
                ["AreaSize"] = 32,
            };
        }

        public static void SetConstants()
        {
            foreach (var property in GameConstantAttribute.GetProperties())
            {
                property.SetValue(null, ConstantValues[property.Name]);
            }
        }
    }
}