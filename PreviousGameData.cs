using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.DFAssist.Keep
{
	class PreviousGameData
	{
		// 버전 목록
		public static readonly GameVersion[] PreviousVersions = new GameVersion[]
		{
			/* 00000000 */ new GameVersion(0, "not-existance"),
			/* 20190628 */ new GameVersion(1, "5.0"),
			/* 20191029 */ new GameVersion(2, "5.1"),
			/* 20191111 */ new GameVersion(3, "5.11"),
			/* 20191126 */ new GameVersion(4, "5.11HF"),
			/* 20191210 */ new GameVersion(5, "5.15"),
			/* 20191224 */ new GameVersion(6, "5.18"),
			/* 20200218 */ new GameVersion(7, "5.2"),
			/* 20200330 */ new GameVersion(8, "5.21HF"),
			/* 20200330 */ new GameVersion(9, "5.25"),
			/* 20200811 */ new GameVersion(10, "5.3"),
			/* 20200825 */ new GameVersion(11, "5.3HF"),
			/* 20200909 */ new GameVersion(12, "5.31"),
			/* 20201013 */ new GameVersion(13, "5.35"),
			/* 20201027 */ new GameVersion(14, "5.35HF"),
			/* 20201110 */ new GameVersion(15, "KR 5.25"),
			/* 20201208 */ new GameVersion(16, "5.4"),
			/* 20201222 */ new GameVersion(17, "5.4HF"),
			/* 20210112 */ new GameVersion(18, "KR 5.3"),
			/* 20210112 */ new GameVersion(19, "5.41"),
			/* 20210202 */ new GameVersion(20, "5.45"),
		};

		//
		public static readonly GamePacket[] PreviousPackets = new GamePacket[]
		{
			/*   0 */ 
			new GamePacket
			{
				Version = "not-existance",
				OpFate = 0,
				FateIndex = 0,
				OpDuty = 0,
				DutyRoulette = 0,
				DutyInstance = 0,
				OpMatch = 0,
				MatchRoulette = 0,
				MatchInstance = 0,
				OpInstance = 0,
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
				OpBozjaCe = 0x03C1,
			},
			/*  17 */ 
			new GamePacket
			{
				Version = "0540HF",
				OpFate = 0x06A,
				FateIndex = 0x35,
				OpDuty = 0x01FB,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x2E0,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x7B,
				OpBozjaCe = 0x31B,
			},	
			/*  18 */ 
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
			/*  19 */ 
			new GamePacket
			{
				Version = "0541",
				OpFate = 0x03A3,
				FateIndex = 0x35,
				OpDuty = 0x02DC,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x01D2,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpInstance = 0x0157,
				OpBozjaCe = 0x0118,
			},
			/*  20 */ 
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
		};
	}
}
