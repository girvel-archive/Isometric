using System;
using System.Linq;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Game.Modules.GameData.Defaults;
using VisualConsole;

namespace Isometric.Client.Modules.BuildingGraphics
{
    public class GraphicsManager
    {
        #region Singleton

        [Obsolete("using backing field")]
        private GraphicsManager _instance;

        #pragma warning disable 618

        public GraphicsManager Instance => _instance ?? (_instance = new GraphicsManager());

        #pragma warning restore 618

        #endregion



        public GraphicsPair[] GraphicsArray { get; set; }



        public GraphicsManager() 
        {
            GraphicsArray = new[]
                {
                    new GraphicsPair(
                        new ConsolePoint('f', ConsoleColor.DarkGreen),
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.Forest)),

                    new GraphicsPair(
                        new ConsolePoint('~', ConsoleColor.DarkBlue),
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.Water)),

                    new GraphicsPair(
                        new ConsolePoint('.', ConsoleColor.DarkMagenta),
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.Plain)),

                    new GraphicsPair(
                        new ConsolePoint('r', ConsoleColor.DarkGray),
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.Rock)),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.WoodHouse)),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        BuildingPatternList.Instance.First(b => b.Name == BuildingPatternNames.WoodHouse2)),
                };
        }



        public IConsolePoint GetGraphics(Building building)
        {
            return GraphicsArray.First(pair => pair.Pattern == building.Pattern).Graphics;
        }
    }
}

