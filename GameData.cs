using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACT.DFAssist
{
    static class GameData
	{
        // 클라이언트 버전
		public class ClientVersion
		{
			public string Name { get; set; }
			public string Value { get; set; }
		}

        // 클라이언트 버전 목록
		public static readonly ClientVersion[] ClientVersions = new ClientVersion[]
		{
			new ClientVersion{Name="5.18", Value="0"},
			new ClientVersion{Name="5.15", Value="5"},
			new ClientVersion{Name="5.11HF", Value="4"},
			new ClientVersion{Name="5.11", Value="3"},
			new ClientVersion{Name="5.1", Value="2"},
			new ClientVersion{Name="<= 5.0", Value="1"},
		};

        // 인스턴스
		public class Instance
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public byte Tank { get; set; }
            public byte Healer { get; set; }
            public byte Dps { get; set; }
            public bool PvP { get; set; }
        }

        // 루렛
        public class Roulette
        {
            public string Name { get; set; }

            public static explicit operator Roulette(string name)
            {
                return new Roulette { Name = name };
            }
        }

        // 지역
        public class Area
        {
            public string Name { get; set; }
            public IReadOnlyDictionary<int, Fate> Fates { get; set; }
        }

        // 페이트
        public class Fate
        {
            public Area Area { get; set; }
            public string Name { get; set; }

            public static explicit operator Fate(string name)
            {
                return new Fate { Name = name };
            }
        }

        // 파일 내 데이터: 버전 - 인스턴스 - 루렛 - 지역
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

        // 초기화
        public static void Initialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            // gamedata_{lang}.json
            Fill(json);
        }

        // json 분석
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
