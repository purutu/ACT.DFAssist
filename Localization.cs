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
        public class Locale
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public static readonly Locale[] Locales = new Locale[]
        {
            new Locale{Name="English", Code="en"},
            new Locale{Name="にほんご", Code="ja"},
            new Locale{Name="Deutsch", Code="de"},
            new Locale{Name="Le français", Code="fr"},
            new Locale{Name="한국말", Code="ko"},
        };

        public static readonly Locale DefaultLocale = new Locale { Name = "English", Code = "en" };

        private static Dictionary<string, string> _map;

        public static void Initialize(string path, string lang)
        {
            var file = string.Concat("locale-", lang, ".json");
            var name = Path.Combine(path, file);

            var json = File.ReadAllText(name, System.Text.Encoding.UTF8);
            _map = string.IsNullOrWhiteSpace(json) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static string GetText(string key, params object[] args)
        {
            return _map == null || !_map.TryGetValue(key, out var value) ? $"<{key}>" : string.Format(value, args);
        }
    }
}
