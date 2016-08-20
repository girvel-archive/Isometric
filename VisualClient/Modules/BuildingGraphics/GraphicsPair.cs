using System;
using VisualConsole;
using IsometricCore.Modules.WorldModule.Buildings;

namespace VisualClient.Modules.BuildingGraphics
{
    public struct GraphicsPair
    {
        public IConsolePoint Graphics { get; set; }
        public BuildingPattern Pattern { get; set; }

        public GraphicsPair(IConsolePoint graphics, BuildingPattern pattern)
        {
            Graphics = graphics;
            Pattern = pattern;
        }
    }
}

