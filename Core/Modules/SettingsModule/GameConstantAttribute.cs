using System;
using System.Linq;
using System.Reflection;
using Isometric.Core.Modules.WorldModule;

namespace Isometric.Core.Modules.SettingsModule
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GameConstantAttribute : Attribute
    {
        public static PropertyInfo[] GetProperties()
        {
            return typeof (World).Assembly
                .GetTypes()
                .SelectMany(type => 
                    type.GetProperties(
                        BindingFlags.NonPublic | 
                        BindingFlags.Public | 
                        BindingFlags.Static))
                .Where(property =>
                    property.GetCustomAttribute<GameConstantAttribute>() != null)
                .ToArray();
        }
    }
}