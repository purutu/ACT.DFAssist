#define COMPAT_4_5
#define COMPAT_5_0
#define COMPAT_5_1
#define COMPAT_5_1_1
#define COMPAT_5_1_1_HF_20191126
#define COMPAT_5_1_5

using System;
using System.Collections.Generic;
using System.Linq;

namespace ACT.DFAssist
{
	internal static partial class PacketWorker
	{
		private static void HandleMessage_0511_HF(byte[] message)
		{
			var opcode = BitConverter.ToUInt16(message, 18);

#if false

#if !DEBUG
			if (
#if COMPAT_4_5 || COMPAT_5_0
					opcode != 0x006F &&
				opcode != 0x0078 &&
				opcode != 0x0079 &&
				opcode != 0x0080 &&
				opcode != 0x0121 &&
				opcode != 0x0143 &&
#endif
#if COMPAT_5_1
				// 5.1
				opcode != 0x008F &&
				opcode != 0x00AE &&
				opcode != 0x00B3 &&
				opcode != 0x015E &&
#endif
#if COMPAT_5_1_1
				// 5.11
				opcode != 0x0002 &&
				opcode != 0x0164 &&
				opcode != 0x0339 &&
				opcode != 0x032D &&
				opcode != 0x032F &&
				opcode != 0x03CF &&
#endif
#if COMPAT_5_1 || COMPAT_5_1_1
				opcode != 0x0304 &&
#endif
#if COMPAT_5_1_1_HF_20191126
				opcode != 0x02B0 &&
#endif
#if COMPAT_5_1_5
				opcode != 0x0135 &&
				opcode != 0x0193 &&
#endif
				opcode != 0x022F)
				return;
#endif

			var data = message.Skip(32).ToArray();

			if (opcode == 0x022F) // 인스턴스 들어오고 나가기
			{
				var code = BitConverter.ToInt16(data, 4);

				if (code == 0)
					return;

				var type = data[8];

				if (type == 0x0B)
				{
					// 들어옴
					MsgLog.Info("l-instance-field-enter", GameData.GetInstanceName(code));
					FireEvent(GameEvents.InstanceEnter, new int[] { code });
				}
				else if (type == 0x0C)
				{
					// 나감
					MsgLog.Info("l-instance-field-left");
					FireEvent(GameEvents.InstanceLeave, new int[] { code });
				}
			} // 22F
#if COMPAT_4_5 || COMPAT_5_0
			else if (opcode == 0x0143) // FATE 관련
			{
#if false
                    // FATE 막음 2019-11-01
                    var type = data[0];

                    if (type == 0x9B)
                    {
                        var code = BitConverter.ToUInt16(data, 4);
                        var progress = data[8];
                        FireEvent(GameEvents.FateProgress, new int[] { code, progress });
                    }
                    else if (type == 0x79) // FATE 끗
                    {
                        var code = BitConverter.ToUInt16(data, 4);
                        var status = BitConverter.ToUInt16(data, 28);
                        FireEvent(GameEvents.FateEnd, new int[] { code, status });
                    }
                    else if (type == 0x74) // FATE 시작! 에이리어 이동해도 진행중인 것도 이걸로 처리됨
                    {
                        var code = BitConverter.ToUInt16(data, 4);

                        if (Settings.LoggingWholeFates || Settings.SelectedFates.Contains(code.ToString()))
                        {
                            MsgLog.Info("l-fate-occured-info", GameData.GetFate(code).Name);
                            FireEvent(GameEvents.FateBegin, new int[] { code });
                        }
                    }
#endif
			} // 143
			else if (opcode == 0x0078) // 5.1이전 듀티
			{
				var status = data[0];
				var reason = data[4];

				if (status == 0) // 듀티 큐
				{
					state = MatchStatus.Queued;

					_rouletteCode = data[20];

					if (_rouletteCode != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
					{
						MsgLog.Info("i-queue-roulette", GameData.GetRouletteName(_rouletteCode));
						FireEvent(GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
					}
					else // 듀티 지정 큐 (Dungeon/Trial/Raid)
					{
						_rouletteCode = 0;

						var instances = new List<int>();

						for (var i = 0; i < 5; i++)
						{
							var code = BitConverter.ToUInt16(data, 22 + i * 2);
							if (code == 0)
								break;

							instances.Add(code);
						}

						if (!instances.Any())
							return;

						var args = new List<int> { (int)MatchType.Assignment, instances.Count };
						foreach (var item in instances)
							args.Add(item);

						MsgLog.Info("i-queue-instance", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
						FireEvent(GameEvents.MatchBegin, args.ToArray());
					}
				}
				else if (status == 3) // 취소
				{
					state = reason == 8 ? MatchStatus.Queued : MatchStatus.Idle;
					MsgLog.Info("l-queue-stopped");
					FireEvent(GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
				}
				else if (status == 6) // 들어가기
				{
					state = MatchStatus.Idle;
					MsgLog.Info("l-queue-entered");
					FireEvent(GameEvents.MatchEnd, new[] { (int)MatchResult.Enter });
				}
				else if (status == 4) // 매치
				{
					var roulette = data[20];
					var code = BitConverter.ToUInt16(data, 22);

					state = MatchStatus.Matched;

					MsgLog.Info("i-matched", GameData.GetInstanceName(code));
					FireEvent(GameEvents.MatchDone, new int[] { roulette, code });
				}
			} // 78
			else if (opcode == 0x006F)
			{
				var status = data[0];

				if (status == 0)
				{
					// 플레이어가 매칭 참가 확인 창에서 취소를 누르거나 참가 확인 제한 시간이 초과됨
					// 매칭 중단을 알리기 위해 상단 2DB status 3 패킷이 연이어 옴
				}
				if (status == 1)
				{
					// 플레이어가 매칭 참가 확인 창에서 확인을 누름
					// 다른 매칭 인원들도 전부 확인을 눌렀을 경우 입장을 위해 상단 2DB status 6 패킷이 옴
					FireEvent(GameEvents.MatchCancel, new int[] { -1 });
				}
			} // 6F
			else if (opcode == 0x0121)  // 글로벌 깜빡
			{
				var status = data[5];

				if (status == 128)
				{
					// 매칭 참가 신청 확인 창에서 확인을 누름, 그러니깐 표시 안하도됨
					FireEvent(GameEvents.MatchCancel, new int[] { -1 });
				}
			} // 121
			else if (opcode == 0x0079) // 매치 상태
			{
				var code = BitConverter.ToUInt16(data, 0);
				var order = data[4];
				var status = data[8];
				var tank = data[9];
				var dps = data[10];
				var healer = data[11];
				var member = tank * 10000 + healer * 100 + dps;
				var instance = GameData.GetInstance(code);

				if (status == 1)
				{
					if (state == MatchStatus.Matched && _lastMember != member)
					{
						// 마지막 정보와 다름, 다른 사람에 의한 취소... 인데 이거 되나??!!!
						state = MatchStatus.Queued;
						FireEvent(GameEvents.MatchCancel, new int[] { -1 });

					}
					else if (state == MatchStatus.Idle || state == MatchStatus.Queued)
					{
						if (state == MatchStatus.Idle)
						{
							// 매칭 중간에 플러그인이 시작됨
							state = MatchStatus.Queued;
						}

						if (_rouletteCode > 0 || (tank == 0 && healer == 0 && dps == 0))
							FireEvent(GameEvents.MatchOrder, new int[] { order });
						else
							FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusShort, code, status, tank, healer, dps });
					}

					_lastMember = member;
					_lastOrder = order;
				}
				else if (status == 2)
				{
					// 매칭 파티의 인원 정보
					FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusShort, code, status, tank, healer, dps });
					return; // 이건 로그를 안뿌린다
				}
				else //if (status == 4)
				{
					// 매칭하고 파티 인원 상태
					// 추가로: 다른거땜에 오버레이가 지워질 수도 있음... ㅠㅠ
					FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusShort, code, status, tank, healer, dps });

					if (status != 4)
					{
						// 기타면 로그 출력X
						// ....그러고보니 status == 2일때도 여기서 처리해도 되는데 지면이 부족하여 패-_-스
						return;
					}
				}

				var memberinfo = $"{order} | {tank}/{instance.Tank}, {healer}/{instance.Healer}, {dps}/{instance.Dps} | {member}";
				MsgLog.Info("l-queue-updated", instance.Name, status, memberinfo);
			} // 79
			else if (opcode == 0x0080)
			{
				var roulette = data[2];
				var code = BitConverter.ToUInt16(data, 4);

				state = MatchStatus.Matched;
				FireEvent(GameEvents.MatchDone, new int[] { roulette, code });

				MsgLog.Success("i-matched ", code);
			} // 80
#endif
#if COMPAT_5_1
#region 5.1 추가
			else if (opcode == 0x008F)    // 5.1 큐 (opcode = 0x0078, status = 0)
			{
				var status = data[0];
				var reason = data[4];

				state = MatchStatus.Queued;

				_rouletteCode = data[8];

				if (_rouletteCode != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
				{
					MsgLog.Info("i-queue-roulette", GameData.GetRouletteName(_rouletteCode));
					FireEvent(GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
				}
				else // 듀티 지정 큐 (Dungeon/Trial/Raid)
				{
					_rouletteCode = 0;

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

					MsgLog.Info("i-queue-instance", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
					FireEvent(GameEvents.MatchBegin, args.ToArray());
				}
			} // 8F
			else if (opcode == 0x00B3)    // 5.1 매칭 (opcode = 0x0078, status = 4)
			{
				var code = BitConverter.ToUInt16(data, 20);

				state = MatchStatus.Matched;

				MsgLog.Info("i-matched", GameData.GetInstanceName(code));
				FireEvent(GameEvents.MatchDone, new int[] { _rouletteCode, code });
			} // B3
			else if (opcode == 0x015E)    // 5.1 캔슬 (opcode = 0x0078, status = 3)
			{
				var status = data[3];

				if (status == 8)    // 0이아님
				{
					state = MatchStatus.Idle;
					MsgLog.Info("l-queue-stopped");
					FireEvent(GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
				}
			} // 15E
			else if (opcode == 0x00AE)  // 5.1 매칭하고 파티 인원 (opcode = 0x0078, status = 4)
			{
				var code = BitConverter.ToUInt16(data, 8);
				var tank = data[12];
				var healer = data[14];
				var dps = data[16];

				FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusShort, code, 0, tank, healer, dps });
			} // AE
#endregion  // 5.1 추가
#endif
#if COMPAT_5_1_1
#region 5.11 추가
			else if (opcode == 0x0339) // 5.11 인스턴스 들어오고 나가기
			{
				var code = BitConverter.ToInt16(data, 4);

				if (code == 0)
					return;

				var type = data[8];

				if (type == 0x0B)
				{
					// 들어옴
					MsgLog.Info("l-instance-field-enter", GameData.GetInstanceName(code));
					FireEvent(GameEvents.InstanceEnter, new int[] { code });
				}
				else if (type == 0x0C)
				{
					// 나감
					MsgLog.Info("l-instance-field-left");
					FireEvent(GameEvents.InstanceLeave, new int[] { code });
				}
			} // 339
			else if (opcode == 0x0164)    // 5.11 큐
			{
				var status = data[0];
				var reason = data[4];

				state = MatchStatus.Queued;

				_rouletteCode = data[8];

				if (_rouletteCode != 0 && (data[15] == 0 || data[15] == 64))
				{
					MsgLog.Info("i-queue-roulette", GameData.GetRouletteName(_rouletteCode));
					FireEvent(GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
				}
				else // 듀티 지정 큐 (Dungeon/Trial/Raid)
				{
					_rouletteCode = 0;

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

					MsgLog.Info("i-queue-instance", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
					FireEvent(GameEvents.MatchBegin, args.ToArray());
				}
			} // 164
			else if (opcode == 0x032D)    // 5.11 매칭
			{
				_rouletteCode = BitConverter.ToUInt16(data, 2);
				var code = BitConverter.ToUInt16(data, 20);

				state = MatchStatus.Matched;

				MsgLog.Info("i-matched", GameData.GetInstanceName(code));
				FireEvent(GameEvents.MatchDone, new int[] { _rouletteCode, code });
			} // 32D
			else if (opcode == 0x03CF)    // 5.11 듀티 상태
			{
				var status = data[0];

				if (status == 0x73) // 매칭 취소
				{
					state = MatchStatus.Idle;
					MsgLog.Info("l-queue-stopped");
					FireEvent(GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
				}
				else if (status == 0x81)    // 매칭 넣었음
				{
					//state = MatchStatus.Idle;
					//FireEvent(GameEvents.MatchEnd, new[] { (int)MatchResult.Enter });
				}
			} // 3CF
			else if (opcode == 0x032F)  // 5.11 매칭하고 파티 인원
			{
				var code = BitConverter.ToUInt16(data, 8);
				var tank = data[12];
				var maxtank = data[13];
				var healer = data[14];
				var maxhealer = data[15];
				var dps = data[16];
				var maxdps = data[17];

				var instance = GameData.GetInstance(code);

				if (instance.PvP ||
					tank > maxtank || healer > maxhealer || dps > maxdps ||
					tank > instance.Tank || healer > instance.Healer || dps > instance.Dps)
				{
					// 이거하면 계속 울려서 안된다..
					//FireEvent(GameEvents.MatchDone, new int[] { _rouletteCode, code });
					FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusCode, code, 0 });
				}
				else
				{
					if ((tank == maxtank && healer == maxhealer && dps == maxdps) ||
						(tank == instance.Tank && healer == instance.Healer && dps == instance.Dps))
					{
						// 입장으로 간주한다
						//MsgLog.Info("l-instance-field-enter", GameData.GetInstanceName(code));
						FireEvent(GameEvents.InstanceEnter, new int[] { code });
					}
					else
					{
						//FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.ShortStatus, code, 0, tank, healer, dps });
						FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusLong, code, 0, tank, healer, dps, maxtank, maxhealer, maxdps });
					}
				}
			} // 32F
			else if (opcode == 0x0002)  // 5.11 매칭 완료
			{
				// 딱히 할건없음
				//FireEvent(GameEvents.MatchCancel, new int[] { -1 });
			} // 2
#endregion  // 5.11 추가
#endif
#if COMPAT_5_1 || COMPAT_5_1_1
#region 5.1 & 5.11
			else if (opcode == 0x0304)  // 5.1, 5.11 상태 (opcode = 0x0078, status = 1)
			{
				var order = data[6];
				var wait = data[7];
				var tank = data[8];
				var maxtank = data[9];
				var healer = data[10];
				var maxhealer = data[11];
				var dps = data[12];
				var maxdps = data[13];
				var member = tank * 10000 + healer * 100 + dps;

				if (state == MatchStatus.Matched && _lastMember != member)
				{
					// 마지막 정보와 다름, 다른 사람에 의한 취소... 인데 이거 되나??!!!
					state = MatchStatus.Queued;
					FireEvent(GameEvents.MatchCancel, new int[] { -1 });

				}
				else if (state == MatchStatus.Idle || state == MatchStatus.Queued)
				{
					if (state == MatchStatus.Idle)
					{
						// 매칭 중간에 플러그인 시작
						state = MatchStatus.Queued;
					}

					if (_rouletteCode > 0 ||
						(tank == 0 && healer == 0 && dps == 0) ||
						(tank > maxtank || healer > maxhealer || dps > maxdps))
						FireEvent(GameEvents.MatchOrder, new int[] { order });
					else
						FireEvent(GameEvents.MatchStatus, new int[] { (int)MatchType.StatusLong, 0, order, tank, healer, dps, maxtank, maxhealer, maxdps });
				}

				_lastMember = member;
				_lastOrder = order;

				var memberinfo = $"{tank}/{maxtank}, {healer}/{maxhealer}, {dps}/{maxdps}";
				MsgLog.Info("l-queue-updated", $"#{order}", wait, memberinfo);
			} // 304
#endregion
#endif
#if COMPAT_5_1_1_HF_20191126
#region 5.11 Hotfix 11/26/2019
			else if (opcode == 0x02B0)
			{
				_rouletteCode = BitConverter.ToUInt16(data, 2);
				var code = BitConverter.ToUInt16(data, 20);

				state = MatchStatus.Matched;

				MsgLog.Info("i-matched", GameData.GetInstanceName(code));
				FireEvent(GameEvents.MatchDone, new int[] { _rouletteCode, code });
			}
#endregion
#endif
#if COMPAT_5_1_5
#region 5.15
			else if (opcode == 0x0193)      // 5.15 듀티 큐
			{
				var status = data[0];
				var reason = data[4];
				var roulette = data[8];

				state = MatchStatus.Queued;

				if (_rouletteCode != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
				{
					MsgLog.Info("i-queue-roulette", GameData.GetRouletteName(_rouletteCode));
					FireEvent(GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
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

					MsgLog.Info("i-queue-instance", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
					FireEvent(GameEvents.MatchBegin, args.ToArray());
				}
			} // 193
			else if (opcode == 0x0135)      // 5.15 매칭
			{
				var roulette = BitConverter.ToUInt16(data, 2);
				var code = BitConverter.ToUInt16(data, 20);

				state = MatchStatus.Matched;

				MsgLog.Info("i-matched", GameData.GetInstanceName(code));
				FireEvent(GameEvents.MatchDone, new int[] { roulette, code });
			} // 135
#endregion
#endif

#endif
		}
	}
}
