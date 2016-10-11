using System;

namespace Isometric.Client.Modules
{
    public class SingleServer
    {
        #region Singleton

        [Obsolete("using backing field")]
        private static Server.Server _instance;

        #pragma warning disable 618

        public static Server.Server Instance
        {
            get { return _instance ?? (_instance = new Server.Server()); }

            set
            {
                #if DEBUG
                if (_instance != null)
                {
                    throw new ArgumentException("Instance is already set");
                }
                #endif

                _instance = value;
            }
        }

        #pragma warning restore 618

        #endregion
    }
}

