using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ACT.DFAssist
{
	internal static partial class PacketFFXIV
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

		private static void FireEvent(int pid, GameEvents gameevent, int[] args)
		{
			OnEventReceived?.Invoke(pid, gameevent, args);
		}

		private static void HandleMessage(int pid, byte[] message, ref MatchStatus state)
		{
			if (message.Length < 32)
			{
				// type == 0x0000 이였던 메시지는 여기서 걸러짐
				return;
			}

			// 하는곳
			try
			{
				// 버전별 분기
				switch (Settings.ClientVersion)
				{
					case 0:
						HandleMessage_Current(pid, message, ref state);
						break;

					default:
						HandleMessage_0511_HF(pid, message, ref state);
						break;
				}
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
#endif
			}
		} // HandleMessage
	}
}
