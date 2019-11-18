using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ACT.DFAssist
{
	internal static class PacketFFXIV
	{
		public delegate void EventHandler(int pid, GameEvents gameevent, int[] args);
		public static event EventHandler OnEventReceived;

		private static int _lastMember = 0;
		private static int _lastOrder = 0;
		private static int _rouletteCode = 0;

		public static void Analyze(int pid, byte[] payload, ref MatchStatus state)
		{
			try
			{
				while (true)
				{
					if (payload.Length < 4)
						break;

					var type = BitConverter.ToUInt16(payload, 0);

					if (type == 0x0000 || type == 0x5252)
					{
						if (payload.Length < 28)
							break;

						var length = BitConverter.ToInt32(payload, 24);

						if (length <= 0 || payload.Length < length)
							break;

						using (var messages = new MemoryStream(payload.Length))
						{
							using (var stream = new MemoryStream(payload, 0, length))
							{
								stream.Seek(40, SeekOrigin.Begin);

								if (payload[33] == 0x00)
									stream.CopyTo(messages);
								else
								{
									stream.Seek(2, SeekOrigin.Current); // 닷넷 DeflateStream 버그 (앞 2바이트 넘겨야함)

									using (var z = new DeflateStream(stream, CompressionMode.Decompress))
										z.CopyTo(messages);
								}
							}
							messages.Seek(0, SeekOrigin.Begin);

							var messageCount = BitConverter.ToUInt16(payload, 30);
							for (var i = 0; i < messageCount; i++)
							{
								try
								{
									var buffer = new byte[4];
									var read = messages.Read(buffer, 0, 4);
									if (read < 4)
									{
										MsgLog.Error("l-analyze-error-length", read, i, messageCount);
										break;
									}
									var messageLength = BitConverter.ToInt32(buffer, 0);

									var message = new byte[messageLength];
									messages.Seek(-4, SeekOrigin.Current);
									messages.Read(message, 0, messageLength);

									HandleMessage(pid, message, ref state);
								}
								catch (Exception ex)
								{
									MsgLog.Exception(ex, "l-analyze-error-general");
								}
							}
						}

						if (length < payload.Length)
						{
							// 더 처리해야 할 패킷이 남아 있음
							payload = payload.Skip(length).ToArray();
							continue;
						}
					}
					else
					{
						// 앞쪽이 잘려서 오는 패킷 workaround
						// 잘린 패킷 1개는 버리고 바로 다음 패킷부터 찾기...
						// TODO: 버리는 패킷 없게 제대로 수정하기
						for (var offset = 0; offset < payload.Length - 2; offset++)
						{
							var possibleType = BitConverter.ToUInt16(payload, offset);
							if (possibleType == 0x5252)
							{
								payload = payload.Skip(offset).ToArray();
								Analyze(pid, payload, ref state);
								break;
							}
						}
					}

					break;
				}
			}
			catch (Exception ex)
			{
				MsgLog.Exception(ex, "l-analyze-error");
			}
		}

		private static void HandleMessage(int pid, byte[] message, ref MatchStatus state)
		{
			try
			{
				if (message.Length < 32)
				{
					// type == 0x0000 이였던 메시지는 여기서 걸러짐
					return;
				}

				var opcode = BitConverter.ToUInt16(message, 18);

#if !DEBUG
				if (
					opcode != 0x006F &&
					opcode != 0x0078 &&
					opcode != 0x0079 &&
					opcode != 0x0080 &&
					opcode != 0x0121 &&
					opcode != 0x0143 &&
					opcode != 0x022F &&
					// 5.1
					opcode != 0x008F &&
					opcode != 0x00AE &&
					opcode != 0x00B3 &&
					opcode != 0x015E &&
					opcode != 0x0304 &&
					// 5.11
					opcode != 0x0002 &&
					opcode != 0x0164 &&
					opcode != 0x0339 &&
					opcode != 0x032D &&
					opcode != 0x032F &&
					opcode != 0x03CF)
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
				else if (opcode == 0x0143) // FATE 관련
				{
#if false
                    // FATE 막음 2019-11-01
                    var type = data[0];

                    if (type == 0x9B)
                    {
                        var code = BitConverter.ToUInt16(data, 4);
                        var progress = data[8];
                        FireEvent(pid, GameEvents.FateProgress, new int[] { code, progress });
                    }
                    else if (type == 0x79) // FATE 끗
                    {
                        var code = BitConverter.ToUInt16(data, 4);
                        var status = BitConverter.ToUInt16(data, 28);
                        FireEvent(pid, GameEvents.FateEnd, new int[] { code, status });
                    }
                    else if (type == 0x74) // FATE 시작! 에이리어 이동해도 진행중인 것도 이걸로 처리됨
                    {
                        var code = BitConverter.ToUInt16(data, 4);

                        if (Settings.LoggingWholeFates || Settings.SelectedFates.Contains(code.ToString()))
                        {
                            MsgLog.Info("l-fate-occured-info", GameData.GetFate(code).Name);
                            FireEvent(pid, GameEvents.FateBegin, new int[] { code });
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
							MsgLog.Info("l-queue-started-roulette", GameData.GetRouletteName(_rouletteCode));
							FireEvent(pid, GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
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

							MsgLog.Info("l-queue-started-general", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
							FireEvent(pid, GameEvents.MatchBegin, args.ToArray());
						}
					}
					else if (status == 3) // 취소
					{
						state = reason == 8 ? MatchStatus.Queued : MatchStatus.Idle;
						MsgLog.Info("l-queue-stopped");
						FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
					}
					else if (status == 6) // 들어가기
					{
						state = MatchStatus.Idle;
						MsgLog.Info("l-queue-entered");
						FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Enter });
					}
					else if (status == 4) // 매치
					{
						var roulette = data[20];
						var code = BitConverter.ToUInt16(data, 22);

						state = MatchStatus.Matched;

						MsgLog.Info("l-queue-matched", GameData.GetInstanceName(code));
						FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });
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
						FireEvent(pid, GameEvents.MatchCancel, new int[] { -1 });
					}
				} // 6F
				else if (opcode == 0x0121)  // 글로벌 깜빡
				{
					var status = data[5];

					if (status == 128)
					{
						// 매칭 참가 신청 확인 창에서 확인을 누름, 그러니깐 표시 안하도됨
						FireEvent(pid, GameEvents.MatchCancel, new int[] { -1 });
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
							FireEvent(pid, GameEvents.MatchCancel, new int[] { -1 });

						}
						else if (state == MatchStatus.Idle || state == MatchStatus.Queued)
						{
							if (state == MatchStatus.Idle)
							{
								// 매칭 중간에 플러그인이 시작됨
								state = MatchStatus.Queued;
							}

							if (_rouletteCode > 0 || (tank == 0 && healer == 0 && dps == 0))
								FireEvent(pid, GameEvents.MatchOrder, new int[] { order });
							else
								FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.ShortStatus, code, status, tank, healer, dps });
						}

						_lastMember = member;
						_lastOrder = order;
					}
					else if (status == 2)
					{
						// 매칭 파티의 인원 정보
						FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.ShortStatus, code, status, tank, healer, dps });
						return; // 이건 로그를 안뿌린다
					}
					else //if (status == 4)
					{
						// 매칭하고 파티 인원 상태
						// 추가로: 다른거땜에 오버레이가 지워질 수도 있음... ㅠㅠ
						FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.ShortStatus, code, status, tank, healer, dps });

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
					FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });

					MsgLog.Success("l-queue-matched ", code);
				} // 80
				#region 5.1 추가
				else if (opcode == 0x008F)    // 5.1 큐 (opcode = 0x0078, status = 0)
				{
					var status = data[0];
					var reason = data[4];

					state = MatchStatus.Queued;

					_rouletteCode = data[8];

					if (_rouletteCode != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
					{
						MsgLog.Info("l-queue-started-roulette", GameData.GetRouletteName(_rouletteCode));
						FireEvent(pid, GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
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

						MsgLog.Info("l-queue-started-general", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
						FireEvent(pid, GameEvents.MatchBegin, args.ToArray());
					}
				} // 8F
				else if (opcode == 0x00B3)    // 5.1 매칭 (opcode = 0x0078, status = 4)
				{
					var code = BitConverter.ToUInt16(data, 20);

					state = MatchStatus.Matched;

					MsgLog.Info("l-queue-matched", GameData.GetInstanceName(code));
					FireEvent(pid, GameEvents.MatchDone, new int[] { _rouletteCode, code });
				} // B3
				else if (opcode == 0x015E)    // 5.1 캔슬 (opcode = 0x0078, status = 3)
				{
					var status = data[3];

					if (status == 8)    // 0이아님
					{
						state = MatchStatus.Idle;
						MsgLog.Info("l-queue-stopped");
						FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
					}
				} // 15E
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
						FireEvent(pid, GameEvents.MatchCancel, new int[] { -1 });

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
							FireEvent(pid, GameEvents.MatchOrder, new int[] { order });
						else
							FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.LongStatus, 0, order, tank, healer, dps, maxtank, maxhealer, maxdps });
					}

					_lastMember = member;
					_lastOrder = order;

					var memberinfo = $"{tank}/{maxtank}, {healer}/{maxhealer}, {dps}/{maxdps}";
					MsgLog.Info("l-queue-updated", $"#{order}", wait, memberinfo);
				} // 304
				else if (opcode == 0x00AE)  // 5.1 매칭하고 파티 인원 (opcode = 0x0078, status = 4)
				{
					var code = BitConverter.ToUInt16(data, 8);
					var tank = data[12];
					var healer = data[14];
					var dps = data[16];

					FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.ShortStatus, code, 0, tank, healer, dps });
				} // AE
				#endregion  // 5.1 추가
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
						MsgLog.Info("l-field-instance-entered", GameData.GetInstanceName(code));
						FireEvent(pid, GameEvents.InstanceEnter, new int[] { code });
					}
					else if (type == 0x0C)
					{
						// 나감
						MsgLog.Info("l-field-instance-left");
						FireEvent(pid, GameEvents.InstanceLeave, new int[] { code });
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
						MsgLog.Info("l-queue-started-roulette", GameData.GetRouletteName(_rouletteCode));
						FireEvent(pid, GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, _rouletteCode });
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

						MsgLog.Info("l-queue-started-general", string.Join(", ", instances.Select(x => GameData.GetInstanceName(x)).ToArray()));
						FireEvent(pid, GameEvents.MatchBegin, args.ToArray());
					}
				} // 164
				else if (opcode == 0x032D)    // 5.11 매칭
				{
					//var roulette = BitConverter.ToUInt16(data, 2);
					var code = BitConverter.ToUInt16(data, 20);

					state = MatchStatus.Matched;

					MsgLog.Info("l-queue-matched", GameData.GetInstanceName(code));
					FireEvent(pid, GameEvents.MatchDone, new int[] { _rouletteCode, code });
				} // 32D
				else if (opcode == 0x03CF)    // 5.11 듀티 상태
				{
					var status = data[0];

					if (status == 0x73) // 매칭 취소
					{
						state = MatchStatus.Idle;
						MsgLog.Info("l-queue-stopped");
						FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
					}
					else if (status == 0x81)    // 매칭 넣었음
					{
						//state = MatchStatus.Idle;
						//FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Enter });
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

					if (tank > maxtank || healer > maxhealer || dps > maxdps)
						FireEvent(pid, GameEvents.MatchDone, new int[] { _rouletteCode, code });
					else
					{
						//FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.ShortStatus, code, 0, tank, healer, dps });
						FireEvent(pid, GameEvents.MatchStatus, new int[] { (int)MatchType.LongStatus, code, 0, tank, healer, dps, maxtank, maxhealer, maxdps });
					}
				} // 32F
				else if (opcode == 0x0002)  // 5.11 매칭 완료
				{
					// 딱히 할건없음
					//FireEvent(pid, GameEvents.MatchCancel, new int[] { -1 });
				} // 2
				#endregion  // 5.11 추가
			}
			catch (Exception ex)
			{
#if false
                MsgLog.Exception(ex, "l-analyze-error-handle");
#else
				// 에이리어 이동할때만 나타난다. 메시지를 출력하지 말자
				// DFASSIST에서는 안나는데 플러그인은 이동만 하면 난다
				var fmt = Localization.GetText("l-analyze-error-handle");
				var msg = MsgLog.Escape(ex.Message);
				System.Diagnostics.Debug.WriteLine($"{fmt}: {msg}");

				// 오버레이는 지워버리는게 좋겠다
				//FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
#endif
			}
		}

		private static void FireEvent(int pid, GameEvents gameevent, int[] args)
		{
			OnEventReceived?.Invoke(pid, gameevent, args);
		}
	}
}
