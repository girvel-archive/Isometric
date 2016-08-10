using System;
using GameCore.Modules.PlayerModule;
using CompressedStructures;

namespace GameCore.Modules.TickModule
{
	public interface IResourcesChanging
	{
		Resources Tick();
	}
}

