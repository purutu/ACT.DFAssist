using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACT.DFAssist
{
	internal static class GameData
	{
		// roulette
		public class Roulette
		{
			public string Name { get; set; }

			public static explicit operator Roulette(string name)
			{
				return new Roulette { Name = name };
			}
		}

		// instance
		public class Instance
		{
			public string Name { get; set; }

			public static explicit operator Instance(string name)
			{
				return new Instance { Name = name };
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

		// group
		public class Group
		{
			public Dictionary<int, Roulette> Roulettes { get; set; }
			public Dictionary<int, Instance> Instances { get; set; }
			public Dictionary<int, Area> Areas { get; set; }
		}

		//
		public static IReadOnlyDictionary<int, Roulette> Roulettes { get; private set; } = new Dictionary<int, Roulette>();
		public static IReadOnlyDictionary<int, Instance> Instances { get; private set; } = new Dictionary<int, Instance>();
		public static IReadOnlyDictionary<int, Area> Areas { get; private set; } = new Dictionary<int, Area>();
		public static IReadOnlyDictionary<int, Fate> Fates { get; private set; } = new Dictionary<int, Fate>();

		// 
		public static void Initialize(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return;

			Fill(json);
		}


		// parse json 
		private static void Fill(string json)
		{
			Group data = JsonConvert.DeserializeObject<Group>(json);

			Dictionary<int, Fate> fates = new Dictionary<int, Fate>();

			foreach (var area in data.Areas)
			{
				foreach (var fate in area.Value.Fates)
				{
					try
					{
						fate.Value.Area = area.Value;
						fates.Add(fate.Key, fate.Value);
					}
					catch (NullReferenceException /*nex*/)
					{
						Mesg.E("e-null-data", fate.Key);
					}
					catch (Exception ex)
					{
						Mesg.Ex(ex, "l-data-error");
					}
				}
			}

			Roulettes = data.Roulettes;
			Instances = data.Instances;
			Areas = data.Areas;
			Fates = fates;
		}

		//
		public static Roulette TryRoulette(int code)
		{
			return Roulettes.TryGetValue(code, out Roulette roulette) ? roulette : null;
		}

		//
		public static Instance TryInstance(int code)
		{
			return Instances.TryGetValue(code, out Instance instance) ? instance : null;
		}

		//
		public static Area TryArea(int code)
		{
			return Areas.TryGetValue(code, out Area area) ? area : null;
		}

		//
		public static Fate TryFate(int code)
		{
			return Fates.ContainsKey(code) ? Fates[code] : null;
		}

		//
		public static Roulette GetRoulette(int code)
		{
			return Roulettes.TryGetValue(code, out Roulette roulette) ? roulette : new Roulette { Name = Mesg.GetText("l-unknown-roulette", code) };
		}

		//
		public static Instance GetInstance(int code)
		{
			return Instances.TryGetValue(code, out Instance instance) ? instance : new Instance { Name = Mesg.GetText("l-unknown-instance", code) };
		}

		//
		public static Area GetArea(int code)
		{
			return Areas.TryGetValue(code, out Area area) ? area : new Area { Name = Mesg.GetText("l-unknown-area", code) };
		}

		//
		public static Fate GetFate(int code)
		{
			return Fates.ContainsKey(code) ? Fates[code] : new Fate { Name = Mesg.GetText("l-unknown-fate", code) };
		}
	}
}
