using CommandInterface;
using CompressedStructures;
using static System.Console;

namespace CommandTester
{
    internal static class Program
    {
        private static void Main(string[] consoleArgs)
        {
            foreach (var element in "([True;nothing])".ParseList(CommonBuildingAction.GetFromString))
            {
                WriteLine(element.Active + "::" + element.Name);
            }

            ReadKey();
        }
    }
}
