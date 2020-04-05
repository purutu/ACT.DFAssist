using Machina.FFXIV;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACT.DFAssist
{
	internal static partial class PacketWorker
	{
		// packet codes to use
		public static GameData.PacketCode Codes { get; set; } = GameData.PacketCodes[0];

		//
		public delegate void EventHandler(string pid, GameEvents gameevent, int[] args);
		public static event EventHandler OnEventReceived;

		//
		private static FFXIVNetworkMonitor _moniter;

		// start machina!
		public static void BeginMachina()
		{
			EndMachina();

			_moniter = new FFXIVNetworkMonitor
			{
				MessageReceived = MachinaWorker
			};
			_moniter.Start();
		}

		// stop machina!
		public static void EndMachina()
		{
			if (_moniter != null)
			{
				_moniter.Stop();
				_moniter = null;
			}
		}

		// worker delegate of machina
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		private static void MachinaWorker(string pid, long epoch, byte[] message)
		{
			if (message.Length < 32)
				return;

			try
			{
				PacketFFXIV(pid, message);
			}
			catch
			{
				//
			}
		}

		//
		private static void FireEvent(string pid, GameEvents gameevent, int[] args)
		{
			OnEventReceived?.Invoke(pid, gameevent, args);
		}

		// process packet
		private static void PacketFFXIV(string pid, byte[] message)
		{
			var opcode = BitConverter.ToUInt16(message, 18);

#if !DEBUG
			if (opcode != Codes.OpFate &&
				opcode != Codes.OpDuty &&
				opcode != Codes.OpMatch &&
				opcode != Codes.OpEnter)
				return;
#endif

			var data = message.Skip(32).ToArray();

			if (opcode == Codes.OpFate)           // FATE 관련
			{
				var type = data[0];

				if (type == Codes.FateIndex) // FATE 시작! 에이리어 이동해도 진행중인 것도 이걸로 처리됨
				{
					var code = BitConverter.ToUInt16(data, 4);

					if (Settings.LoggingWholeFates || Settings.SelectedFates.Contains(code.ToString()))
					{
						MsgLog.Fate("l-fate-occured-info", GameData.GetFate(code).Name);
						FireEvent(pid, GameEvents.FateOccur, new int[] { code });
					}
				}
			} // Codes.FATE
			else if (opcode == Codes.OpDuty)  // 듀티
			{
				var status = data[0];
				var reason = data[4];
				var roulette = data[Codes.DutyRoulette];

				if (roulette != 0)  // (roulette != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
				{
					MsgLog.Duty("i-queue-roulette", GameData.GetRouletteName(roulette));
					FireEvent(pid, GameEvents.MatchQueue, new[] { (int)MatchType.Roulette, roulette });
				}
				else // 골라놓은 듀티 큐 (Dungeon/Trial/Raid)
				{
					var instances = new List<int>();

					for (var i = 0; i < 5; i++)
					{
						var code = BitConverter.ToUInt16(data, Codes.DutyInstance + (i * 4));
						if (code == 0)
							break;
					}

					if (!instances.Any())
						return;

					var args = new List<int> { (int)MatchType.Assignment, instances.Count };
					foreach (var item in instances)
						args.Add(item);

					MsgLog.Duty("i-queue-instance", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
					FireEvent(pid, GameEvents.MatchQueue, args.ToArray());
				}
			} // Codes.Duty
			else if (opcode == Codes.OpMatch) // 매칭
			{
				var roulette = BitConverter.ToUInt16(data, Codes.MatchRoulette);
				var instance = BitConverter.ToUInt16(data, Codes.MatchInstance);

				MsgLog.Duty("i-matched", $"{GameData.GetInstanceName(instance)}/{GameData.GetRouletteName(roulette)}");
				FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, instance });
			} // Codes.Match
			else if (opcode == Codes.OpEnter && Codes.OpEnter != 0) // 입장
			{
				if (data[4] == 0)
				{
					var instance = BitConverter.ToUInt16(data, Codes.EnterInstance);
					MsgLog.Duty("i-matched", GameData.GetInstanceName(instance));
					FireEvent(pid, GameEvents.MatchEnter, new int[] { instance });
				}
			}
		}
	}
}
