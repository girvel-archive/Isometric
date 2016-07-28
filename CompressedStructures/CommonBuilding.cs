using CommandInterface;
using System;
using System.Text;
using VectorNet;

namespace CompressedStructures
{
    [Serializable]
    public class CommonBuilding : ICompressable
    {
        public string Name { get; set; }
        public IntVector Position { get; set; }



        public byte[] GetBytes => Encoding.ASCII.GetBytes(GetString);

        public string GetString => $"[{Name};{Position.GetString}]";



        public CommonBuilding(string name, IntVector position)
        {
            Name = name;
            Position = position;
        }

        public CommonBuilding() { }



        public static CommonBuilding GetFromBytes(byte[] bytes)
            => GetFromString(Encoding.ASCII.GetString(bytes));

        public static CommonBuilding GetFromString(string @string)
        {
            var data = @string.ParseType("[name;position]");
            return new CommonBuilding(
                data["name"],
                IntVector.GetFromBytes(Encoding.ASCII.GetBytes(data["position"])));
        }
    }
}