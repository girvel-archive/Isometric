using System;
using System.Linq;
using System.Reflection;

namespace Isometric.Core.Modules.SettingsModule
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GameConstantAttribute : Attribute
    {
        public static PropertyInfo[] GetProperties(Assembly assembly)
        {
            return assembly
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