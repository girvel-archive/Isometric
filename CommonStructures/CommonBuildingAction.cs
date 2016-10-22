using System;

namespace Isometric.CommonStructures
{
    [Serializable]
    public struct CommonBuildingAction
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public int UpgradeTo { get; set; }



        public CommonBuildingAction(bool active, string name, int upgradeTo)
        {
            Active = active;
            Name = name;
            UpgradeTo = upgradeTo;
        }
    }
}
