using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ACT.DFAssist
{
    static class PacketFFXIV
    {
        public delegate void EventHandler(int pid, GameEvents gameevent, int[] args);
        public static event EventHandler OnEventReceived;

        private static int _lastMember;

        public static void Analyze(int pid, byte[] payload, ref State state)
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

        private static void HandleMessage(int pid, byte[] message, ref State state)
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
                if (opcode != 0x0078 &&
                    opcode != 0x0079 &&
                    opcode != 0x0080 &&
                    opcode != 0x006C &&
                    opcode != 0x006F &&
                    opcode != 0x0121 &&
                    opcode != 0x0143 &&
                    opcode != 0x022F)
                    return;
#endif

                var data = message.Skip(32).ToArray();

                if (opcode == 0x022F) // 인스턴스 들어오고 나가기
                {
                    var code = BitConverter.ToInt16(data, 4);
                    var type = data[8];

                    if (type == 0x0B)
                    {
                        MsgLog.Info("l-field-instance-entered", GameData.GetInstance(code).Name);
                        FireEvent(pid, GameEvents.InstanceEnter, new int[] { code });
                    }
                    else if (type == 0x0C)
                    {
                        MsgLog.Info("l-field-instance-left");
                        FireEvent(pid, GameEvents.InstanceLeave, new int[] { code });
                    }
                }
                else if (opcode == 0x0143) // FATE 진행
                {
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
                        MsgLog.Info("l-fate-occured-info", GameData.GetFate(code).Name);
                        FireEvent(pid, GameEvents.FateBegin, new int[] { code });
                    }
                }
                else if (opcode == 0x0078) // Duties
                {
                    var status = data[0];
                    var reason = data[4];

                    if (status == 0) // 듀티 큐
                    {
                        state = State.Queued;

                        var rouletteCode = data[20];

                        if (rouletteCode != 0 && (data[15] == 0 || data[15] == 64)) // 루렛, 한국/글로벌
                        {
                            MsgLog.Info("l-queue-started-roulette", GameData.GetRoulette(rouletteCode).Name);
                            FireEvent(pid, GameEvents.MatchBegin, new[] { (int)MatchType.Roulette, rouletteCode });
                        }
                        else // 듀티 지정 큐 (Dungeon/Trial/Raid)
                        {
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

                            MsgLog.Info("l-queue-started-general", string.Join(", ", instances.Select(x => GameData.GetInstance(x).Name).ToArray()));
                            FireEvent(pid, GameEvents.MatchBegin, args.ToArray());
                        }
                    }
                    else if (status == 3) // 취소
                    {
                        state = reason == 8 ? State.Queued : State.Idle;
                        MsgLog.Info("l-queue-stopped");
                        FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Cancel });
                    }
                    else if (status == 6) // 들어가기
                    {
                        state = State.Idle;
                        MsgLog.Info("l-queue-entered");
                        FireEvent(pid, GameEvents.MatchEnd, new[] { (int)MatchResult.Enter });
                    }
                    else if (status == 4) // 매치
                    {
                        var roulette = data[20];
                        var code = BitConverter.ToUInt16(data, 22);

                        state = State.Matched;

                        MsgLog.Info("l-queue-matched", GameData.GetInstance(code).Name);
                        FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });
                    }
                }
                else if (opcode == 0x006F)
                {
                    // DFASSIST 깜빡임용
                }
                else if (opcode == 0x0121)
                {
                    // DFASSIST 깜박임 중지용. 글로벌용이라고 함
                }
                else if (opcode == 0x0079) // 매치 상태
                {
                    var code = BitConverter.ToUInt16(data, 0);
                    var status = data[4];
                    var tank = data[5];
                    var dps = data[6];
                    var healer = data[7];
#if false
                    var instance = GameData.GetInstance(code, true); // 예전 아이디 사용
#else
                    var instance = GameData.GetInstance(code);
#endif

                    if (status == 1)
                    {
                        var member = tank * 10000 + dps * 100 + healer;

                        if (state == State.Matched && _lastMember != member)
                        {
                            // 마지막 정보와 다름
                            state = State.Queued;
                        }
                        else if (state == State.Idle)
                        {
                            // 매칭 중간에 플러그리인이 시작됨
                            state = State.Queued;
                        }

                        _lastMember = member;
                    }
                    else if (status == 2)
                    {
                        // 매칭 파티의 인원 정보
                        return;
                    }
                    else if (status == 4)
                    {
                        // 매칭하고 파티 인원 상태
                    }

                    MsgLog.Info("l-queue-updated", instance.Name, status, tank, instance.Tank, healer, instance.Healer, dps, instance.Dps);
                    FireEvent(pid, GameEvents.MatchStatus, new int[] { code, status, tank, healer, dps });
                }
                else if (opcode == 0x0080)
                {
                    var roulette = data[2];
                    var code = BitConverter.ToUInt16(data, 4);

                    state = State.Matched;

                    MsgLog.Success("l-queue-matched ", code);
                    FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });
                }
            }
            catch (Exception ex)
            {
#if false
                MsgLog.Exception(ex, "l-analyze-error-handle");
#else
                // 지역이동할때만 나타난다. 메시지를 출력하지 말자
                var fmt = Localization.GetText("l-analyze-error-handle");
                var msg = MsgLog.Escape(ex.Message);
                System.Diagnostics.Debug.WriteLine($"{fmt}: {msg}");
#endif
            }
        }

        private static void FireEvent(int pid, GameEvents gameevent, int[] args)
        {
            OnEventReceived?.Invoke(pid, gameevent, args);
        }
    }
}
