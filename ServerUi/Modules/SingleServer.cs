using System;
using System.Linq;
using Isometric.Game.Modules;
using Isometric.Server;
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

        static SingleServer() {
            Connection.OnConnectionEnd += _connectionEnd;
        }

        private static void _connectionEnd(Connection connection)
        {
            SinglePlayersManager.Instance.Players.First(p => p.Name == connection.Account.Login).OnTick -=
                connection.SendResources;
        }
    }
}

