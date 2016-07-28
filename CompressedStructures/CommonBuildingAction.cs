using CommandInterface;
using System;
using System.Text;

namespace CompressedStructures
{
    [Serializable]
    public class CommonBuildingAction : ICompressable
    {
        public bool Active { get; set; }
        public string Name { get; set; }



        public byte[] GetBytes => Encoding.ASCII.GetBytes(GetString);

        public string GetString => $"[{Active};{Name}]";



        public CommonBuildingAction(bool active, string name)
        {
            Active = active;
            Name = name;
        }

        public CommonBuildingAction() {}



        public static CommonBuildingAction GetFromString(string @string)
        {
            var parts = @string.ParseType("[active;name]");
            return new CommonBuildingAction(
                bool.Parse(parts["active"]),
                parts["name"]);
        }

        public static CommonBuildingAction GetFromBytes(byte[] bytes) 
            => GetFromString(Encoding.ASCII.GetString(bytes));
    }
}
