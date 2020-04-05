using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ACT.DFAssist.Toolkits;
using Newtonsoft.Json;

namespace ACT.DFAssist
{
	static class Settings
	{
		public static string TagName = "20200220";  // 20200220 -> 20200405

		public static string Path { get; set; }
		public static readonly ConcurrentHashSet<string> SelectedFates = new ConcurrentHashSet<string>();

		public static bool LoggingWholeFates { get; set; }
		public static bool UseOverlay { get; set; }
		public static Point OverlayLocation { get; set; } = new Point(0, 0);

		public static string PluginPath { get; set; } = "";

		public static bool AutoHideOverlay { get; set; }

		public static int ClientVersion { get; set; } = 0;


		//
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		internal static string GetTagNameForUpdate()
		{
			string url = "https://api.github.com/repos/purutu/ACT.DFAssist/releases/latest";
			var rq = Toolkits.HttpHelper.Request(url);

			if (rq != null)
			{
				try
				{
					var js = JsonConvert.DeserializeObject<dynamic>(rq);
					var tagname = js.tag_name.ToObject<string>();   // "tag_name": "20200113"

					return tagname;
				}
				catch
				{
				}
			}

			// 확인못하면 체크 못하게 현재 태그 날림
			return TagName;
		}
	}
}
