using System;
using GameCore.Modules.PlayerModule;

namespace GameCore.Modules.TickModule
{
	public interface IResourcesBonusChanging
	{
		void Tick(ref Resources resources);
	}
}

