using System;
using GameBasics;
using GameBasics.PlayerClasses;

namespace GameRealization.PlayerClasses.ProgressElements
{
    /// <summary>
    /// <c>ProgressElement</c>
    /// </summary>
    [Serializable]
    public class Hunting : ProgressElement
    {
        public override string Name => "Hunters' stories";
        public override int ProgressPointsMax => 5;



        public Hunting(Player owner) : base(owner) {}



        public override void Refresh() {}
    }
}

