using Machina.FFXIV;

namespace ACT.DFAssist
{
	internal static partial class PacketWorker
	{
		//
		public delegate void EventHandler(string pid, GameEvents gameevent, int[] args);
		public static event EventHandler OnEventReceived;

		//
		private static FFXIVNetworkMonitor _moniter;

		// 머시나 시작
		public static void BeginMachina()
		{
			EndMachina();

			_moniter = new FFXIVNetworkMonitor();
			_moniter.MessageReceived = MachinaWorker;
			_moniter.Start();
		}

		// 머시나 끝
		public static void EndMachina()
		{
			if (_moniter != null)
			{
				_moniter.Stop();
				_moniter = null;
			}
		}

		//
		private static void FireEvent(string pid, GameEvents gameevent, int[] args)
		{
			OnEventReceived?.Invoke(pid, gameevent, args);
		}

		// 머시나 워커
		private static void MachinaWorker(string pid, long epoch, byte[] message)
		{
			if (message.Length < 32)
				return;

			try
			{
				// 버전별 분기
				switch (Settings.ClientVersion)
				{
					default:
						//HandleMessage_0511_HF(message);
						PacketFFXIV_518(pid, message);
						break;
				}
			}
			catch
			{
				//
			}
		}
	}
}
