using System;
using System.Text;

namespace CommonStructures
{
    [Serializable]
    public class CommonBuildingAction
    {
        public bool Active { get; set; }
        public string Name { get; set; }



        public CommonBuildingAction(bool active, string name)
        {
            Active = active;
            Name = name;
        }

        [Obsolete("using serialization ctor", true)]
        public CommonBuildingAction() {}
    }
}
