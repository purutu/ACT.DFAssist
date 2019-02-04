using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.DFAssist
{
    class Localization
    {
        public class Language
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        private static Dictionary<string, string> _map;

        public static void Initialize(string path, string lang)
        {
            var file = string.Concat("lang-", lang, ".json");
            var name = Path.Combine(path, file);

            var json = File.ReadAllText(name, System.Text.Encoding.UTF8);
            _map =string.IsNullOrWhiteSpace(json) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static string GetText(string key, params object[] args)
        {
            return _map == null || !_map.TryGetValue(key, out var value) ? $"<{key}>" : string.Format(value, args);
        }
    }
}
