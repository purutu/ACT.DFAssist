using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACT.DFAssist
{
	static class GameData
	{
		// client version 
		public class ClientVersion
		{
			public string Name { get; set; }
			public int Value { get; set; }
		}

		// client version list 
		public static readonly ClientVersion[] ClientVersions = new ClientVersion[]
		{
			/* 20200330 */ new ClientVersion{Name = "5.21HF", Value = 0},
			/* 20200218 */ new ClientVersion{Name = "5.2", Value = 7},
			/* 20191224 */ new ClientVersion{Name = "5.18", Value = 6},
			/* 20191210 */ new ClientVersion{Name = "5.15", Value = 5},
			/* 20191126 */ new ClientVersion{Name = "5.11HF", Value = 4},
			/* 20191111 */ new ClientVersion{Name = "5.11", Value = 3},
			/* 20191029 */ new ClientVersion{Name = "5.1", Value = 2},
			/* 20190628 */ new ClientVersion{Name = "<= 5.0", Value = 1},
		};

		// packet code
		public struct PacketCode
		{
			public string Version { get; set; }			// 게임 버전
			public ushort OpFate { get; set; }			// FATE op
			public ushort FateIndex { get; set; }		// 페이트 인덱스
			public ushort OpDuty { get; set; }          // 임무 op
			public ushort DutyRoulette { get; set; }    // 임무 룰렛 인덱스
			public ushort DutyInstance { get; set; }    // 임무 인스턴스 인덱스
			public ushort OpMatch { get; set; }         // 매치 op
			public ushort MatchRoulette { get; set; }   // 매치 룰렛 인덱스
			public ushort MatchInstance { get; set; }   // 매치 인스턴스 인덱스
			public ushort OpEnter { get; set; }			// 입장
			public ushort EnterInstance { get; set; }	// 임장 인스턴스 인덱스
		}

		// packet code list, reverse index of client version!!!, 0 is always up to date code
		public static readonly PacketCode[] PacketCodes = new PacketCode[]
		{
			/*   0 */ 
			new PacketCode
			{
				Version = "Current",
				OpFate = 0x0253,
				FateIndex = 0x35,	// 페이트 인덱스는 35아니면 3E 둘중 하나인데 35로함
				OpDuty = 0x0164,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x02C7,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpEnter = 0x12F,
				EnterInstance = 0,
			},
																			  				 
			/*   1 */
			new PacketCode
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
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   2 */
			new PacketCode
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
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   3 */
			new PacketCode
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
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   4 */
			new PacketCode
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
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   5 */
			new PacketCode
			{
				Version = "Current",
				OpFate = 0x00E3,
				FateIndex = 0x74,
				OpDuty = 0x0193,
				DutyRoulette = 8,
				DutyInstance = 12,
				OpMatch = 0x0135,
				MatchRoulette = 2,
				MatchInstance = 20,
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   6 */ 
			new PacketCode
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
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   7 */ 
			new PacketCode
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
				OpEnter = 0,
				EnterInstance = 0,
			},
			/*   8 */
			new PacketCode
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
				OpEnter = 0x12F,
				EnterInstance = 0,
			},
		};

		// instance
		public class Instance
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public byte Tank { get; set; }
			public byte Healer { get; set; }
			public byte Dps { get; set; }
			public bool PvP { get; set; }
		}

		// roulette
		public class Roulette
		{
			public string Name { get; set; }

			public static explicit operator Roulette(string name)
			{
				return new Roulette { Name = name };
			}
		}

		// area
		public class Area
		{
			public string Name { get; set; }
			public IReadOnlyDictionary<int, Fate> Fates { get; set; }
		}

		// fate
		public class Fate
		{
			public Area Area { get; set; }
			public string Name { get; set; }

			public static explicit operator Fate(string name)
			{
				return new Fate { Name = name };
			}
		}

		// file data grou: version - instance - roulette - area
		public class Group
		{
			public decimal Version { get; set; }
			public Dictionary<int, Instance> Instances { get; set; }
			public Dictionary<int, Roulette> Roulettes { get; set; }
			public Dictionary<int, Area> Areas { get; set; }
		}

		//
		public static decimal Version { get; private set; }

		public static IReadOnlyDictionary<int, Area> Areas { get; private set; } = new Dictionary<int, Area>();
		public static IReadOnlyDictionary<int, Instance> Instances { get; private set; } = new Dictionary<int, Instance>();
		public static IReadOnlyDictionary<int, Roulette> Roulettes { get; private set; } = new Dictionary<int, Roulette>();
		public static IReadOnlyDictionary<int, Fate> Fates { get; private set; } = new Dictionary<int, Fate>();

		// 
		public static void Initialize(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return;

			// gamedata_{lang}.json
			Fill(json);
		}


		// parse json 
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		private static void Fill(string json)
		{
			try
			{
				var data = JsonConvert.DeserializeObject<Group>(json);
				var version = data.Version;

				if (version > decimal.Truncate(Version))
				{
					var fates = new Dictionary<int, Fate>();

					foreach (var area in data.Areas)
					{
						foreach (var fate in area.Value.Fates)
						{
							fate.Value.Area = area.Value;
							fates.Add(fate.Key, fate.Value);
						}
					}

					Areas = data.Areas;
					Instances = data.Instances;
					Roulettes = data.Roulettes;
					Fates = fates;
					Version = version;
				}
			}
			catch (Exception ex)
			{
				MsgLog.Ex(ex, "l-data-error");
			}
		}

		//
		public static Instance TryInstance(int code, bool refId = false)
		{
			if (refId)
				return Instances.Values.FirstOrDefault(x => x.Id == code);

			return Instances.TryGetValue(code, out var instance) ? instance : null;
		}

		//
		public static Roulette TryRoulette(int code)
		{
			return Roulettes.TryGetValue(code, out var roulette) ? roulette : null;
		}

		//
		public static Area TryArea(int code)
		{
			return Areas.TryGetValue(code, out var area) ? area : null;
		}

		//
		public static Fate TryFate(int code)
		{
			return Fates.ContainsKey(code) ? Fates[code] : null;
		}

		//
		public static Instance GetInstance(int code, bool refId = false)
		{
			if (refId)
				return Instances.Values.FirstOrDefault(x => x.Id == code);

			return Instances.TryGetValue(code, out var instance) ? instance : new Instance { Name = Localization.GetText("l-unknown-instance", code) };
		}

		//
		public static Roulette GetRoulette(int code)
		{
			return Roulettes.TryGetValue(code, out var roulette) ? roulette : new Roulette { Name = Localization.GetText("l-unknown-roulette", code) };
		}

		//
		public static Area GetArea(int code)
		{
			return Areas.TryGetValue(code, out var area) ? area : new Area { Name = Localization.GetText("l-unknown-area", code) };
		}

		//
		public static Fate GetFate(int code)
		{
			return Fates.ContainsKey(code) ? Fates[code] : new Fate { Name = Localization.GetText("l-unknown-fate", code) };
		}

		//
		public static string GetInstanceName(int code)
		{
			return GetInstance(code).Name;
		}

		//
		public static string GetFateName(int code)
		{
			return GetFate(code).Name;
		}

		//
		public static string GetAreaNameFromFate(int code)
		{
			return GetFate(code).Area.Name;
		}

		//
		public static string GetRouletteName(int code)
		{
			return GetRoulette(code).Name;
		}
	}
}
