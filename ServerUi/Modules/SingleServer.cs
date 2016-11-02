using System;
using Isometric.Game.Modules;
using _Server = Isometric.Server.Server;

namespace Isometric.Client.Modules
{
    public class SingleServer
    {
        private static _Server _instance;

        public static _Server Instance
        {
            get
            {
                return _instance ?? (_instance
                    = new _Server(
                        SingleWorld.Instance,
                        SinglePlayersManager.Instance,
                        SingleMailManager.Instance,
                        SingleRequestManager.Instance,
                        SingleBuildingGraph.Instance));
            }

            set
            {
#if DEBUG
                if (_instance != null)
                {
                    throw new NotImplementedException("SingleServer.Instance is already set");
                }
#endif
                _instance = value;
            }
        }
    }
}

