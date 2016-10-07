using VisualConsole;

namespace Isometric.Client.Modules
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
//                    Program.Area.BuildingGrid.TwoDimSelect(b => GraphicsManager.Instance.GetGraphics(b));
//                },
//                new List<Command>
        }
    }
}

