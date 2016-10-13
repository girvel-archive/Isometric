using System;
using System.Linq;

namespace Isometric.CommonStructures
{
    [Serializable]
    public struct Resources
    {
        private int[] _resourcesArray;
        public int[] ResourcesArray
        {
            get { return _resourcesArray ?? (_resourcesArray = new int[Enum.GetValues(typeof (ResourceType)).Length]); }

            set
            {
                _resourcesArray = value;
            }
        }

        public int Gold {
            get {
                return ResourcesArray[(byte)ResourceType.Gold];
            }

            set {
                ResourcesArray[(byte)ResourceType.Gold] = value;
            }
        }

        public int Meat {
            get {
                return ResourcesArray[(byte)ResourceType.Meat];
            }

            set {
                ResourcesArray[(byte)ResourceType.Meat] = value;
            }
        }

        public int Corn {
            get {
                return ResourcesArray[(byte)ResourceType.Corn];
            }

            set {
                ResourcesArray[(byte)ResourceType.Corn] = value;
            }
        }

        public int Stone {
            get {
                return ResourcesArray[(byte)ResourceType.Stone];
            }

            set {
                ResourcesArray[(byte)ResourceType.Stone] = value;
            }
        }

        public int Wood {
            get {
                return ResourcesArray[(byte)ResourceType.Wood];
            }

            set {
                ResourcesArray[(byte)ResourceType.Wood] = value;
            }
        }

        public int People {
            get {
                return ResourcesArray[(byte)ResourceType.People];
            }

            set {
                ResourcesArray[(byte)ResourceType.People] = value;
            }
        }



        public bool Empty => _resourcesArray == null || _resourcesArray.All(r => r == 0);



        public Resources(
            int gold = 0, 
            int meat = 0, 
            int corn = 0, 
            int stone = 0, 
            int wood = 0, 
            int people = 0)
        {
            _resourcesArray = new int[Enum.GetValues(typeof(ResourceType)).Length];

            Gold = gold;
            Meat = meat;
            Corn = corn;
            Stone = stone;
            Wood = wood;
            People = people;
        }



        public bool Enough(Resources resources)
        {
            return !ResourcesArray.Where((t, i) => t < resources.ResourcesArray[i]).Any();
        }


        public static Resources operator +(Resources r1, Resources r2)
        {
            var result = new Resources();

            if (r1._resourcesArray == null)
            {
                return r2;
            }

            if (r2._resourcesArray == null)
            {
                return r1;
            }

            for (var i = 0; i < result.ResourcesArray.Length; i++)
            {
                result.ResourcesArray[i] = r1.ResourcesArray[i] + r2.ResourcesArray[i];
            }

            return result;
        }

        public static Resources operator -(Resources r1, Resources r2)
        {
            var result = new Resources();

            for (var i = 0; i < result.ResourcesArray.Length; i++)
            {
                result.ResourcesArray[i] = r1.ResourcesArray[i] - r2.ResourcesArray[i];
            }

            return result;
        }



        public override bool Equals(object obj)
        {
            if (!(obj is Resources))
            {
                return base.Equals(obj);
            }

            var r2 = (Resources)obj;

            if (r2.Empty ^ Empty)
            {
                return false;
            }

            if (r2.Empty && Empty)
            {
                return true;
            }

            return !ResourcesArray.Where((t, i) => t != r2.ResourcesArray[i]).Any();
        }

        public static bool operator ==(Resources r1, Resources r2) => r1.Equals(r2);

        public static bool operator !=(Resources r1, Resources r2) => !(r1 == r2);
    }
}

