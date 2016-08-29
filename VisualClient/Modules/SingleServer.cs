using System;
using VisualServer;

namespace VisualClient.Modules
{
    public class SingleServer
    {
        #region Singleton

        [Obsolete("using backing field")]
        private static Server _instance;

        #pragma warning disable 618

        public static Server Instance
        {
            get { return _instance ?? (_instance = new Server()); }

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

