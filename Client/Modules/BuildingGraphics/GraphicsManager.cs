using System;
using System.Linq;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Implementation.Modules.GameData;
using VisualConsole;

namespace Isometric.Client.Modules.BuildingGraphics
{
    public class GraphicsManager
    {
        #region Singleton

        [Obsolete("using backing field")]
        private GraphicsManager _instance;

        #pragma warning disable 618

        public GraphicsManager Instance
        {
            get { return _instance ?? (_instance = new GraphicsManager()); }

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
                        GameDataManager.Instance.GetPattern("Forest")),

                    new GraphicsPair(
                        new ConsolePoint('~', ConsoleColor.DarkBlue),
                        GameDataManager.Instance.GetPattern("Water")),

                    new GraphicsPair(
                        new ConsolePoint('.', ConsoleColor.DarkMagenta),
                        GameDataManager.Instance.GetPattern("Plain")),

                    new GraphicsPair(
                        new ConsolePoint('r', ConsoleColor.DarkGray),
                        GameDataManager.Instance.GetPattern("Rock")),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        GameDataManager.Instance.GetPattern("WoodHouse")),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        GameDataManager.Instance.GetPattern("WoodHouse2")),
                };
        }



        public IConsolePoint GetGraphics(Building building)
        {
            return GraphicsArray.First(pair => pair.Pattern == building.Pattern).Graphics;
        }
    }
}

