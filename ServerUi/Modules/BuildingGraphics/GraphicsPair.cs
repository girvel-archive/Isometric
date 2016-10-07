using Isometric.Core.Modules.WorldModule.Buildings;
using VisualConsole;

namespace Isometric.Client.Modules.BuildingGraphics
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

