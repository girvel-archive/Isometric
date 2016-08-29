﻿using VisualConsole;

namespace VisualClient.Modules
{
    public static class SingleUI
    {
        private static ConsoleUI _instance;
        public static ConsoleUI Instance => _instance ?? (_instance = _getInstance());

        private static ConsoleUI _getInstance() 
        {
            return new ConsoleUI();
//                getGrid = () =>
//                {
//                    Program.Territory.BuildingGrid.TwoDimSelect(b => GraphicsManager.Instance.GetGraphics(b));
//                },
//                new List<Command>
        }
    }
}

