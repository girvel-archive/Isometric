using System;
using VisualConsole;
using GameCore.Modules.WorldModule.Buildings;
using GameRealization.Modules.PatternsRealization;
using System.Linq;

namespace VisualClient.Modules.BuildingGraphics
{
    public class GraphicsManager
    {
        #region Singleton

        [Obsolete("using backing field")]
        private GraphicsManager _instance;

        #pragma warning disable 618

        public GraphicsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GraphicsManager();
                }

                return _instance;
            }

            set
            {
                #if DEBUG
                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }
                #endif

                _instance = value;
            }
        }

        #pragma warning restore 618

        #endregion



        public GraphicsPair[] GraphicsArray { get; set; }



        public GraphicsManager() 
        {
            GraphicsArray = new[]
                {
                    new GraphicsPair(
                        new ConsolePoint('f', ConsoleColor.DarkGreen), 
                        BuildingPatterns.Forest),

                    new GraphicsPair(
                        new ConsolePoint('~', ConsoleColor.DarkBlue),
                        BuildingPatterns.Water),

                    new GraphicsPair(
                        new ConsolePoint('.', ConsoleColor.DarkMagenta),
                        BuildingPatterns.Plain),

                    new GraphicsPair(
                        new ConsolePoint('r', ConsoleColor.DarkGray),
                        BuildingPatterns.Rock),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        BuildingPatterns.WoodHouse),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        BuildingPatterns.WoodHouse2),
                };
        }



        public IConsolePoint GetGraphics(Building building)
        {
            return GraphicsArray.First(pair => pair.Pattern == building.Pattern).Graphics;
        }
    }
}

