using System;

namespace GameCore.Extensions
{
	public static class DelegateExtensions
	{
		[Serializable]
		public sealed class ExceptionEventArgs : EventArgs
		{
			public Exception Exception { get; }

			public ExceptionEventArgs (Exception exception)
			{
				this.Exception = exception;
			}
		}



		public static void SafeInvoke<T>(
			this EventHandler<T> @event, object sender, T args, EventHandler<ExceptionEventArgs> @catch)
		{
			try
			{
				@event(sender, args);
			}
			catch (Exception ex)
			{
				@catch?.Invoke(sender, new ExceptionEventArgs(ex));
			}
		}
	}
}

