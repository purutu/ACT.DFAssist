using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ACT.DFAssist
{
	internal static partial class PacketFFXIV
	{
		private static void HandleMessage_Current(int pid, byte[] message, ref MatchStatus state)
		{
			var opcode = BitConverter.ToUInt16(message, 18);

#if !DEBUG
			if (opcode != 0x01F8 &&
				opcode != 0x0228 &&
				opcode != 0x022F)
				return;
#endif

			var data = message.Skip(32).ToArray();

			if (opcode == 0x022F)           // 인스턴스 들어오고 나가기
			{
				// [12/15/2019] 이거 동작 안하는듯 
				var code = BitConverter.ToInt16(data, 4);
				var type = data[8];

				if (type == 0x0B)
				{
					// 들어옴
					MsgLog.Info("l-field-instance-entered", GameData.GetInstanceName(code));
					FireEvent(pid, GameEvents.InstanceEnter, new int[] { code });
				}
				else if (type == 0x0C)
				{
					// 나감
					MsgLog.Info("l-field-instance-left");
					FireEvent(pid, GameEvents.InstanceLeave, new int[] { code });
				}
			} // 22F
			else if (opcode == 0x0228)      // 5.18 듀티 큐
			{
				var status = data[0];
				var reason = data[4];
				var roulette = data[8];

				state = MatchStatus.Queued;

				if (_rouletteCode != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
				{
					MsgLog.Info("l-queue-started-roulette", GameData.GetRouletteName(_rouletteCode));
					FireEvent(pid, GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
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

					MsgLog.Info("l-queue-started-general", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
					FireEvent(pid, GameEvents.MatchBegin, args.ToArray());
				}
			} // 193
			else if (opcode == 0x01F8)		// 5.18 매칭
			{
				var roulette = BitConverter.ToUInt16(data, 2);
				var code = BitConverter.ToUInt16(data, 20);

				state = MatchStatus.Matched;

				MsgLog.Info("l-queue-matched", GameData.GetInstanceName(code));
				FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });
			} // 135
		}
	}
}
