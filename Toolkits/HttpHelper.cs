using System;
using System.IO;
using System.Net;
using System.Text;

namespace ACT.DFAssist.Toolkits
{
	public static class HttpHelper
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		public static string Request(string url)
		{
			try
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				ServicePointManager.DefaultConnectionLimit = 9999;

				var req = WebRequest.CreateHttp(url);
				req.UserAgent = "HttpHelper";
				req.Timeout = 10000;
				req.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

				using (var rp = req.GetResponse())
				{
					using (var st = rp.GetResponseStream())
					{
						using (var rdr = new StreamReader(st))
							return rdr.ReadToEnd();
					}
				}
			}
			catch
			{
			}

			return null;
		}

		public static string Request(string fmt, params object[] prms)
		{
			return Request(string.Format(fmt, prms));
		}
	}
}
