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

                        if (Settings.LoggingWholeFates || Settings.SelectedFates.Contains(code.ToString()))
                        {
                            MsgLog.Info("l-fate-occured-info", GameData.GetFate(code).Name);
                            FireEvent(pid, GameEvents.FateBegin, new int[] { code });
                        }
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
                        FireEvent(pid, GameEvents.MatchCancel, new int[] { status });
                    }
                }
                else if (opcode == 0x0121)  // 글로벌 깜빡
                {
                    var status = data[5];

                    if (status == 128)
                    {
                        // 매칭 참가 신청 확인 창에서 확인을 누름, 그러니깐 표시 안하도됨
                        FireEvent(pid, GameEvents.MatchCancel, new int[] { status });
                    }
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
                    var member = 0;

                    if (status == 1)
                    {
                        member = tank * 10000 + healer * 100 + dps;

                        if (state == State.Matched && _lastMember != member)
                        {
                            // 마지막 정보와 다름, 다른 사람에 의한 취소... 인데 이거 되나??!!!
                            state = State.Queued;
                            FireEvent(pid, GameEvents.MatchCancel, new int[] { code, status, tank, healer, dps });

                        }
                        else if (state == State.Idle)
                        {
                            // 매칭 중간에 플러그리인이 시작됨
                            state = State.Queued;
                            FireEvent(pid, GameEvents.MatchCount, new int[] { -1 });
                            FireEvent(pid, GameEvents.MatchStatus, new int[] { code, status, tank, healer, dps });
                        }
                        else if (state == State.Queued)
                        {
                            FireEvent(pid, GameEvents.MatchStatus, new int[] { code, status, tank, healer, dps });
                        }

                        _lastMember = member;
                    }
                    else if (status == 2)
                    {
                        // 매칭 파티의 인원 정보
                        // 이벤트 바꿔야함... 멤머 숫자로만
                        FireEvent(pid, GameEvents.MatchStatus, new int[] { code, status, tank, healer, dps });
                        return;
                    }
                    else if (status == 4)
                    {
                        // 매칭하고 파티 인원 상태
                        // 이벤트 바꿔야함... 컨펌 상태로만
                        FireEvent(pid, GameEvents.MatchStatus, new int[] { code, status, tank, healer, dps });
                    }
                    else
                    {
                        // 다른거땜에 오버레이가 지워질 수도 있음... ㅠㅠ
                        FireEvent(pid, GameEvents.MatchStatus, new int[] { code, status, tank, healer, dps });
                    }

                    var memberinfo = $"{tank}/{instance.Tank}, {healer}/{instance.Healer}, {dps}/{instance.Dps} | {member}";
                    MsgLog.Info("l-queue-updated", instance.Name, status, memberinfo);
                }
                else if (opcode == 0x0080)
                {
                    var roulette = data[2];
                    var code = BitConverter.ToUInt16(data, 4);

                    state = State.Matched;
                    FireEvent(pid, GameEvents.MatchDone, new int[] { roulette, code });

                    MsgLog.Success("l-queue-matched ", code);
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
