using System;
using System.Collections.Generic;
using System.Linq;

namespace ACT.DFAssist
{
	internal static partial class PacketWorker
	{
		private static void PacketFFXIV_518(string pid, byte[] message)
		{
			var opcode = BitConverter.ToUInt16(message, 18);

#if !DEBUG
			if (opcode != 0x00E3 &&
				opcode != 0x01F8 &&
				opcode != 0x0228 &&
				opcode != 0x022F)
				return;
#endif

			var data = message.Skip(32).ToArray();

			if (opcode == 0x00E3) // FATE 관련
			{
				var type = data[0];

                if (type == 0x74) // FATE 시작! 에이리어 이동해도 진행중인 것도 이걸로 처리됨
                {
                    var code = BitConverter.ToUInt16(data, 4);

                    if (Settings.LoggingWholeFates || Settings.SelectedFates.Contains(code.ToString()))
                    {
                        MsgLog.Fate("l-fate-occured-info", GameData.GetFate(code).Name);
                        FireEvent(pid, GameEvents.FateOccur, new int[] { code });
                    }
                }
			} // E3
			else if (opcode == 0x022F)           // 인스턴스 들어오고 나가기
			{
				// [12/15/2019] 이거 동작 안하는듯 
				var code = BitConverter.ToInt16(data, 4);
				var type = data[8];

				if (type == 0x0B)
				{
					// 들어옴
					MsgLog.Instance("l-instance-enter", GameData.GetInstanceName(code));
					FireEvent(pid, GameEvents.InstanceEnter, new int[] { code });
				}
				else if (type == 0x0C)
				{
					// 나감
					MsgLog.Instance("l-instance-leave");
					FireEvent(pid, GameEvents.InstanceLeave, new int[] { code });
				}
			} // 0x022F
			else if (opcode == 0x0228)      // 5.18 듀티 큐
			{
				var status = data[0];
				var reason = data[4];
				var roulette = data[8];

				if (roulette != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
				{
					MsgLog.Duty("i-queue-roulette", GameData.GetRouletteName(roulette));
					FireEvent(pid, GameEvents.MatchQueue, new[] { (int)MatchType.Roulette, roulette });
				}
				else // 골라놓은 듀티 큐 (Dungeon/Trial/Raid)
				{
					var instances = new List<int>();

					for (var i = 0; i < 5; i++)
					{
						var code = BitConverter.ToUInt16(data, 12 + (i * 4));
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
			} // 0x0228
			else if (opcode == 0x01F8)		// 5.18 매칭
			{
				var roulette = BitConverter.ToUInt16(data, 2);
				var code = BitConverter.ToUInt16(data, 20);

				MsgLog.Duty("i-matched", GameData.GetInstanceName(code));
				FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });
			} // 0x01F8
		}
	}
}
