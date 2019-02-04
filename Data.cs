using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACT.DFAssist
{
    static class Data
    {
        public static decimal Version { get; private set; }

        public static IReadOnlyDictionary<int, DataModel.Area> Areas { get; private set; } = new Dictionary<int, DataModel.Area>();
        public static IReadOnlyDictionary<int, DataModel.Instance> Instances { get; private set; } = new Dictionary<int, DataModel.Instance>();
        public static IReadOnlyDictionary<int, DataModel.Roulette> Roulettes { get; private set; } = new Dictionary<int, DataModel.Roulette>();
        public static IReadOnlyDictionary<int, DataModel.Fate> Fates { get; private set; } = new Dictionary<int, DataModel.Fate>();

        public static void Initialize(string path, string lang)
        {
            var file = string.Concat("data-", lang, ".json");
            var name = Path.Combine(path, file);
            var json = File.ReadAllText(name, System.Text.Encoding.UTF8);
            Fill(json);
        }

        private static void Fill(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                var data = JsonConvert.DeserializeObject<DataModel.GameData>(json);
                var version = data.Version;

#if false
                if (version > Version)
#endif
                {
                    var fates = new Dictionary<int, DataModel.Fate>();
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
                MsgLog.Exception(ex, "l-data-error");
            }
        }

        public static DataModel.Instance GetInstance(int code, bool oldId = false)
        {
            if (oldId)
                return Instances.Values.FirstOrDefault(x => x.OldId == code);

            return Instances.TryGetValue(code, out var instance) ? instance : new DataModel.Instance { Name = Localization.GetText("l-unknown-instance", code) };
        }

        public static DataModel.Roulette GetRoulette(int code)
        {
            return Roulettes.TryGetValue(code, out var roulette) ? roulette : new DataModel.Roulette { Name = Localization.GetText("l-unknown-roulette", code) };
        }

        public static DataModel.Area GetArea(int code)
        {
            return Areas.TryGetValue(code, out var area) ? area : new DataModel.Area { Name = Localization.GetText("l-unknown-area", code) };
        }

        public static DataModel.Fate GetFate(int code)
        {
            return Fates.ContainsKey(code) ? Fates[code] : new DataModel.Fate { Name = Localization.GetText("l-unknown-fate", code) };
        }
    }
}
