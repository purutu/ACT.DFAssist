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
				Version = "0545HF",
				OpFate = 0x3D5,
				FateIndex = 53,
				OpDuty = 0x307,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x26E,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x10C,
				OpBozjaCe = 0x1F5,
			},
			/*  1 */ 
			new GamePacket
			{
				Version = "0531KR",
				OpFate = 0x2FC,
				FateIndex = 53,
				OpDuty = 0x6A,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x134,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x1E0,
				OpBozjaCe = 0x0,
			},
		};

		// 현재 패킷
		public static GamePacket Current { get; set; } = Packets[0];
	}
}
