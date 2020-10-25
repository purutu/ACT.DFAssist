using Advanced_Combat_Tracker;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ACT.DFAssist
{
	internal class Mesg
	{
		// 로캘
		public class Locale
		{
			public string Name { get; set; }
			public string Code { get; set; }

			public Locale(string name, string code)
			{
				Name = name;
				Code = code;
			}
		}

		// 로캘 목록
		public static readonly Locale[] Locales = new Locale[]
		{
			new Locale("English", "en"),
			new Locale("にほんご","ja"),
			new Locale("한국말", "ko"),
		};

		// 라인디비
		public static Toolkits.LineDb _db;

		// 초기화
		public static void Initialize(string text)
		{
			_db = new Toolkits.LineDb(text);
		}

		//
		public static string GetText(string text, params object[] args)
		{
			return _db == null || !_db.Try(text, out string value) ? $"<{text}>" : string.Format(value, args);
		}

		// 메시지 카테고리
		public enum Category
		{
			Info,
			Error,
			Debug,

			Duty,
			FATE,

			SK,
			CE,
		}

		// 네이티브
		private static class NativeMethods
		{
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
			private const int WM_VSCROLL = 277;
			private const int SB_PAGEBOTTOM = 7;

			internal static void ScrollToBottom(RichTextBox richTextBox)
			{
				SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
				richTextBox.SelectionStart = richTextBox.Text.Length;
			}
		}

		// 내부 출력용
		private static readonly Regex EscapePattern = new Regex(@"\{(.+?)\}");
		private static RichTextBox _logbox;

		// 리치 박스 설정
		public static void SetTextBox(RichTextBox rtb)
		{
			_logbox = rtb;
		}

		//
		private static void Write(Category cat, Color color, string format, params object[] args)
		{
			if (_logbox == null || _logbox.IsDisposed)
				return;

			string fmt = format ?? "(null)";

			try
			{
				fmt = string.Format(fmt.ToString(), args);
			}
			catch (FormatException)
			{
			}

			string dt = DateTime.Now.ToString("HH:mm:ss");
			string ms = $"[{dt}/{cat}] {fmt}{Environment.NewLine}";

			ActGlobals.oFormActMain.Invoke(new Action(() =>
			{
				_logbox.SelectionColor = color;
				_logbox.SelectionStart = _logbox.TextLength;
				_logbox.SelectionLength = 0;
				_logbox.AppendText(ms);

				_logbox.SelectionColor = _logbox.ForeColor;
				NativeMethods.ScrollToBottom(_logbox);
			}));
		}

		public static void I(string key, params object[] args)
		{
			Write(Category.Info, Color.Black, GetText(key, args));
		}

		public static void E(string key, params object[] args)
		{
			Write(Category.Error, Color.Red, GetText(key, args));
		}

		public static void D(string key, params object[] args)
		{
			Write(Category.Debug, Color.Gray, GetText(key, args));
		}

		public static void Ex(Exception ex, string key, params object[] args)
		{
			string fmt = GetText(key);
			string msg = EscapePattern.Replace(ex.Message, "{{$1}}");
			E($"{fmt}: {msg}", args);
		}

		public static void Duty(string key, params object[] args)
		{
			Write(Category.Duty, Color.Black, GetText(key, args));
		}

		public static void Fate(string key, params object[] args)
		{
			Write(Category.FATE, Color.Black, GetText(key, args));
		}

		public static void Skirmish(string key, params object[] args)
		{
			Write(Category.SK, Color.Black, GetText(key, args));
		}

		public static void CriticalEngagement(string key, params object[] args)
		{
			Write(Category.CE, Color.Black, GetText(key, args));
		}
	}
}
