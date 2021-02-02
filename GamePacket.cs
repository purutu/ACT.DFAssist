namespace ACT.DFAssist
{
	internal class GamePacket
	{
		public string Version { get; set; }                     // 게임 버전
		public ushort OpFate { get; set; }                      // FATE op
		public ushort FateIndex { get; set; }                   // 페이트 인덱스
		public ushort OpDuty { get; set; }                      // 임무 op
		public ushort DutyRoulette { get; set; }                // 임무 룰렛 인덱스
		public ushort DutyInstance { get; set; }                // 임무 인스턴스 인덱스
		public ushort OpMatch { get; set; }                     // 매치 op
		public ushort MatchRoulette { get; set; }               // 매치 룰렛 인덱스
		public ushort MatchInstance { get; set; }               // 매치 인스턴스 인덱스
		public ushort OpInstance { get; set; } = 0;             // 인스턴스
		public ushort InstanceInstance { get; set; } = 0;       // 인스턴스 인스턴스 인덱스
		public ushort OpBozjaCe { get; set; } = 0;				// 남부 보즈야 전선 크리티컬 인게이지먼트

		//
		public static readonly GamePacket[] Packets = new GamePacket[]
		{
			/*   0 */ 
			new GamePacket
			{
				Version = "0545",
				OpFate = 0x350,
				FateIndex = 53,
				OpDuty = 0x15C,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x14C,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x2EE,
				OpBozjaCe = 0x3E1,
			},
			/*  1 */ 
			new GamePacket
			{
				Version = "0530KR",
				OpFate = 0x025F,
				FateIndex = 0x35,
				OpDuty = 0x0081,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x022C,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x02C3,
				OpBozjaCe = 0,
			},
		};

		// 현재 패킷
		public static GamePacket Current { get; set; } = Packets[0];
	}
}
