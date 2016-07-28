using System;
using CompressedStructures;
using GameBasics;

namespace GameRealization.PlayerClasses.Leaders
{
    [Serializable]
    public class MeatLeader : ResourceLeader
    {
        public override ResourceType Type => ResourceType.Meat;
        public override float Bonus => 0.1f;

        public MeatLeader(Player owner) : base(owner) { }
    }
}

