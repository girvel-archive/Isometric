using System;

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
                return _resourcesArray[(byte)ResourceType.Gold];
            }

            set {
                _resourcesArray[(byte)ResourceType.Gold] = value;
            }
        }

        public int Meat {
            get {
                return _resourcesArray[(byte)ResourceType.Meat];
            }

            set {
                _resourcesArray[(byte)ResourceType.Meat] = value;
            }
        }

        public int Corn {
            get {
                return _resourcesArray[(byte)ResourceType.Corn];
            }

            set {
                _resourcesArray[(byte)ResourceType.Corn] = value;
            }
        }

        public int Stone {
            get {
                return _resourcesArray[(byte)ResourceType.Stone];
            }

            set {
                _resourcesArray[(byte)ResourceType.Stone] = value;
            }
        }

        public int Wood {
            get {
                return _resourcesArray[(byte)ResourceType.Wood];
            }

            set {
                _resourcesArray[(byte)ResourceType.Wood] = value;
            }
        }

        public int People {
            get {
                return _resourcesArray[(byte)ResourceType.People];
            }

            set {
                _resourcesArray[(byte)ResourceType.People] = value;
            }
        }



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
            for (var i = 0; i < ResourcesArray.Length; i++)
            {
                if (ResourcesArray[i] < resources.ResourcesArray[i])
                {
                    return false;
                }
            }

            return true;
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
    }
}

