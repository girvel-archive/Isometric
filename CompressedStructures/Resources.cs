using System;

namespace CommonStructures
{
	[Serializable]
	public struct Resources
	{
        public int[] ResourcesArray;

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



		public Resources(
			int gold = 0, 
			int meat = 0, 
			int corn = 0, 
			int stone = 0, 
			int wood = 0, 
			int people = 0)
		{
            ResourcesArray = new int[typeof(ResourceType).GetEnumValues().Length];

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

