using System;

namespace GameCore.Modules.PlayerModule
{
	[Serializable]
	public struct Resources
	{
		public readonly int Gold, Meat, Corn, Stone, Wood, People;



		public Resources(
			int gold = 0, 
			int meat = 0, 
			int corn = 0, 
			int stone = 0, 
			int wood = 0, 
			int people = 0)
		{
			Gold = gold;
			Meat = meat;
			Corn = corn;
			Stone = stone;
			Wood = wood;
			People = people;
		}

		public static Resources operator +(Resources r1, Resources r2)
		{
			return new Resources(
				r1.Gold + r2.Gold,
				r1.Meat + r2.Meat,
				r1.Corn + r2.Corn,
				r1.Stone + r2.Stone,
				r1.Wood + r2.Wood,
				r1.People + r2.People);
		}

		public static Resources operator -(Resources r1, Resources r2)
		{
			return new Resources(
				r1.Gold - r2.Gold,
				r1.Meat - r2.Meat,
				r1.Corn - r2.Corn,
				r1.Stone - r2.Stone,
				r1.Wood - r2.Wood,
				r1.People - r2.People);
		}
	}
}

