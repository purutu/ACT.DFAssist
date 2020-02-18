using System;
using System.ComponentModel;

namespace ACT.DFAssist.Toolkits
{
	public static class SynchronizeInvoker
	{
		public static void InvokeIfRequired<T>(this T obj, Action<T> action)
			where T : ISynchronizeInvoke
		{
			if (obj.InvokeRequired)
				obj.Invoke(action, new object[] { obj });
			else
				action(obj);
		}

		public static TOut InvokeIfRequired<TIn, TOut>(this TIn obj, Func<TIn, TOut> func)
			where TIn : ISynchronizeInvoke
		{
			return obj.InvokeRequired
				? (TOut)obj.Invoke(func, new object[] { obj })
				: func(obj);
		}
	}
}
