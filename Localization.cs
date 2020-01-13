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
		// locale
		public class Locale
		{
			public int Index { get; set; }
			public string Name { get; set; }
			public string Code { get; set; }
		}

		// locale list
		public static readonly Locale[] Locales = new Locale[]
		{
			new Locale{Index=0, Name="English", Code="en"},
			new Locale{Index=1, Name="にほんご", Code="ja"},
			new Locale{Index=2, Name="Deutsch", Code="de"},
			new Locale{Index=3, Name="Le français", Code="fr"},
			new Locale{Index=4, Name="한국말", Code="ko"},
		};

		// default locale
		public static readonly Locale DefaultLocale = new Locale { Index = 0, Name = "English", Code = "en" };

		//
		private static Dictionary<string, string> _map;

		//
		public static void Initialize(string json)
		{
			// locale_{lang}.json
			_map = string.IsNullOrWhiteSpace(json) ?
				new Dictionary<string, string>() :
				JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
		}

		//
		public static string GetText(string key, params object[] args)
		{
			return _map == null || !_map.TryGetValue(key, out var value) ? $"<{key}>" : string.Format(value, args);
		}
	}
}
