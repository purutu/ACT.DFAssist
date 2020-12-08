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
		public static readonly GamePacket[] Versions = new GamePacket[]
		{
			/*   0 */ 
			new GamePacket
			{
				Version = "Current",
				OpFate = 0x318,
				FateIndex = 0x35,
				OpDuty = 0x369,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x1F6,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0xB0,
				OpBozjaCe = 0,
			},
																			  				 
			/*   1 */
			new GamePacket
			{
				Version = "0500",
				OpFate = 0x0143,
				FateIndex = 0x74,
				OpDuty = 0x0078,
				DutyRoulette = 20,
				DutyInstance = 12,
				OpMatch = 0x0080,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   2 */
			new GamePacket
			{
				Version = "0510",
				OpFate = 0x00E3,
				FateIndex = 0x74,
				OpDuty = 0x008F,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x00B3,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   3 */
			new GamePacket
			{
				Version = "0511",
				OpFate = 0x00E3,
				FateIndex = 0x74,
				OpDuty = 0x0164,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x032D,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   4 */
			new GamePacket
			{
				Version = "0511HF",
				OpFate = 0x00E3,
				FateIndex = 0x35,
				OpDuty = 0x0164,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x02B0,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   5 */
			new GamePacket
			{
				Version = "0511",
				OpFate = 0x00E3,
				FateIndex = 0x74,
				OpDuty = 0x0193,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x0135,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   6 */ 
			new GamePacket
			{
				Version = "0518",
				OpFate = 0x00E3,
				FateIndex = 0x74,
				OpDuty = 0x0228,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x01F8,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   7 */ 
			new GamePacket
			{
				Version = "0520",
				OpFate = 0x010E,
				FateIndex = 0x35,
				OpDuty = 0x0172,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x025C,
				MatchRoulette = 2,
				MatchInstance = 20,
			},
			/*   8 */
			new GamePacket
			{
				Version = "0521HF",
				OpFate = 0x0253,
				FateIndex = 0x35,
				OpDuty = 0x0164,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x02C7,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x12F,
			},
			/*   9 */ 
			new GamePacket
			{
				Version = "0525",
				OpFate = 0x0165,
				FateIndex = 0x35,
				OpDuty = 0x0230,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x0145,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x398,
			},
			/*  10 */ 
			new GamePacket
			{
				Version = "0530",
				OpFate = 0x0212,
				FateIndex = 0x35,
				OpDuty = 0x03CF,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x00FD,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x0167,
			},
			/*  11 */ 
			new GamePacket
			{
				Version = "0530HF",
				OpFate = 0x02B3,
				FateIndex = 0x35,
				OpDuty = 0x038A,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x0278,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x0385,
			},			
			/*  12 */ 
			new GamePacket
			{
				Version = "0531",
				OpFate = 0x020D,
				FateIndex = 0x35,
				OpDuty = 0x02A8,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x0078,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x00BC,
			},			
			/*  13 */ 
			new GamePacket
			{
				Version = "0535",
				OpFate = 0x032C,
				FateIndex = 0x35,
				OpDuty = 0x0283,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x016F,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x029C,
				OpBozjaCe=0x299,
			},
			/*  14 */ 
			new GamePacket
			{
				Version = "0535HF",
				OpFate = 0x02C8,
				FateIndex = 0x35,
				OpDuty = 0x03DB,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x02C4,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x010B,
				OpBozjaCe = 0x0143,
			},			
			/*  15 */ 
			new GamePacket
			{
				Version = "0525KR",
				OpFate = 0x0200,
				FateIndex = 0x35,
				OpDuty = 0x0156,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x0242,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0,
				OpBozjaCe = 0,
			},
			/*  16 */ 
			new GamePacket
			{
				Version = "0540",
				OpFate = 0x318,
				FateIndex = 0x35,
				OpDuty = 0x369,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x1F6,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0xB0,
				OpBozjaCe = 0,
			},
		};

		// 현재 패킷
		public static GamePacket Current { get; set; } = Versions[0];
	}
}
