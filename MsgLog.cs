using Advanced_Combat_Tracker;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ACT.DFAssist
{
    public static class MsgLog
    {
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

        private static readonly Regex EscapePattern = new Regex(@"\{(.+?)\}");

        private static RichTextBox _logbox;

        public static void SetTextBox(RichTextBox logbox)
        {
            _logbox = logbox;
        }

        private static void Write(Color color, object format, params object[] args)
        {
            if (_logbox == null || _logbox.IsDisposed)
                return;

            var formatted = format ?? "(null)";

            try
            {
                formatted = string.Format(formatted.ToString(), args);
            }
            catch (FormatException)
            {
            }

            var datetime = DateTime.Now.ToString("HH:mm:ss");
            var message = $"[{datetime}] {formatted}{Environment.NewLine}";

            ActGlobals.oFormActMain.Invoke(new Action(() =>
            {
                _logbox.SelectionColor = color;
                _logbox.SelectionStart = _logbox.TextLength;
                _logbox.SelectionLength = 0;
                _logbox.AppendText(message);

                _logbox.SelectionColor = _logbox.ForeColor;
                NativeMethods.ScrollToBottom(_logbox);
            }));
        }

        public static void Success(string key, params object[] args)
        {
            Write(Color.Green, Localization.GetText(key, args));
        }

        public static void Info(string key, params object[] args)
        {
            Write(Color.Black, Localization.GetText(key, args));
        }

        public static void Error(string key, params object[] args)
        {
            Write(Color.Red, Localization.GetText(key, args));
        }

        public static void Exception(Exception ex, string key, params object[] args)
        {
            var fmt = Localization.GetText(key);
            var msg = Escape(ex.Message);

            Error($"{fmt}: {msg}", args);
        }

        public static void Debug(object format, params object[] args)
        {
            Write(Color.Gray, format, args);
        }

        public static void Buffer(byte[] buffer)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();

            for (var i = 0; i < buffer.Length; i++)
            {
                if (i != 0)
                {
                    if (i % 16 == 0)
                    {
                        stringBuilder.AppendLine();
                    }
                    else if (i % 8 == 0)
                    {
                        stringBuilder.Append(' ', 2);
                    }
                    else
                    {
                        stringBuilder.Append(' ');
                    }
                }

                stringBuilder.Append(buffer[i].ToString("X2"));
            }

            Debug(stringBuilder.ToString());
        }

        internal static string Escape(string line)
        {
            return EscapePattern.Replace(line, "{{$1}}");
        }
    }
}
