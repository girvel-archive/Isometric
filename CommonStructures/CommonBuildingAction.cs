using System;

namespace CommonStructures
{
    [Serializable]
    public struct CommonBuildingAction
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public CommonBuilding Subject { get; set; }
        public short UpgradeTo { get; set; }



        public CommonBuildingAction(
                bool active, string name, CommonBuilding subject, short upgradeTo)
        {
            Active = active;
            Name = name;
            Subject = subject;
            UpgradeTo = upgradeTo;
        }
    }
}
