using System;
using GameCore.Modules.PlayerModule;

namespace GameCore.Modules.TickModule
{
	public interface IResourcesChanging
	{
		Resources Tick();
	}
}

