using System;
using CompressedStructures;
using GameBasics;
using GameBasics.PlayerClasses;

namespace GameRealization.PlayerClasses.Leaders
{
    [Serializable]
    public abstract class ResourceLeader : Leader
    {
        public abstract ResourceType Type { get; }
        public abstract float Bonus { get; }

        protected ResourceLeader(Player owner) : base(owner) { }

        //public override void Refresh()
        //{
        //    base.Refresh();

        //    Owner.Resources.BonusPercent[ResourceType.Meat] += Bonus;
        //}
    }
}

