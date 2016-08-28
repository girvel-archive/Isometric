using System;

namespace CommonStructures
{
    [Serializable]
    public class CommonBuildingAction
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public CommonBuilding Subject { get; set; }
        public short Upgrade { get; set; }



        public CommonBuildingAction(
                bool active, string name, CommonBuilding subject, short upgrade)
        {
            Active = active;
            Name = name;
            Subject = subject;
            Upgrade = upgrade;
        }

        [Obsolete("using serialization ctor", true)]
        public CommonBuildingAction() {}
    }
}
