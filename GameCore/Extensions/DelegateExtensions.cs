using System;

namespace GameCore.Extensions
{
    public static class DelegateExtensions
    {
        public static void SafeInvoke(Action action, Action<Exception> @catch)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                @catch?.Invoke(ex);
            }
        }
    }
}

