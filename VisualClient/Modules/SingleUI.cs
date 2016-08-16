using System;
using VisualConsole;
using CommandInterface;
using System.Collections.Generic;
using VisualServer.Extensions;
using VisualClient.Modules.BuildingGraphics;

namespace VisualClient.Modules
{
    public static class SingleUI
	{
        private static ConsoleUI _instance;
        public static ConsoleUI Instance {
            get {
                if (_instance == null)
                {
                    _instance = _getInstance();
                }

                return _instance;
            }
        }

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

