using System;
using System.Linq;
using Isometric.Core.Modules;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Implementation.Modules.GameData;
using Isometric.Implementation.Modules.GameData.Defaults;
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
                        MainBuildingList.Instance.First(b => b.Name == BuildingNames.Forest)),

                    new GraphicsPair(
                        new ConsolePoint('~', ConsoleColor.DarkBlue),
                        MainBuildingList.Instance.First(b => b.Name == BuildingNames.Water)),

                    new GraphicsPair(
                        new ConsolePoint('.', ConsoleColor.DarkMagenta),
                        MainBuildingList.Instance.First(b => b.Name == BuildingNames.Plain)),

                    new GraphicsPair(
                        new ConsolePoint('r', ConsoleColor.DarkGray),
                        MainBuildingList.Instance.First(b => b.Name == BuildingNames.Rock)),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        MainBuildingList.Instance.First(b => b.Name == BuildingNames.WoodHouse)),

                    new GraphicsPair(
                        new ConsolePoint('H', ConsoleColor.Yellow),
                        MainBuildingList.Instance.First(b => b.Name == BuildingNames.WoodHouse2)),
                };
        }



        public IConsolePoint GetGraphics(Building building)
        {
            return GraphicsArray.First(pair => pair.Pattern == building.Pattern).Graphics;
        }
    }
}

