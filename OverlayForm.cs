using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ACT.DFAssist
{
	public partial class OverlayForm : Form
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
			[DllImport("user32.dll")]
			public static extern bool ReleaseCapture();
		}

		private const int WS_EX_LAYERED = 0x80000;
		private const int WS_EX_TOOLWINDOW = 0x80;
		private const int WM_NCLBUTTONDOWN = 0xA1;
		private const int HT_CAPTION = 0x2;

		//
		private const int BlinkTime = 300;
		private const int BlinkCount = 20;

		private static readonly Color ColorFate = Color.DarkOrange;
		private static readonly Color ColorMatch = Color.Red;

		//
		private readonly Timer _timer;
		private int _blink;
		private Color _accent_color;

		//
		public OverlayForm()
		{
			InitializeComponent();

			Location = Settings.OverlayLocation;

			_timer = new Timer
			{
				Interval = BlinkTime
			};
			_timer.Tick += Timer_Tick;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (++_blink > BlinkCount)
				StopBlink();
			else
			{
				if (BackColor == Color.Black)
					BackColor = _accent_color;
				else
					BackColor = Color.Black;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS_EX_LAYERED | WS_EX_TOOLWINDOW;
				return cp;
			}
		}

		private void OverlayForm_Load(object sender, EventArgs e)
		{

		}

		private void DoMoveDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				NativeMethods.ReleaseCapture();
				NativeMethods.SendMessage(Handle, WM_NCLBUTTONDOWN, new IntPtr(HT_CAPTION), IntPtr.Zero);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			DoMoveDown(e);
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);

			Settings.OverlayLocation = Location;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
		}

		private void LblInfo_MouseDown(object sender, MouseEventArgs e)
		{
			DoMoveDown(e);
		}

		public void SetInfoText(string localtext)
		{
			lblInfo.Text = Mesg.GetText(localtext);
		}

		internal void StartBlink()
		{
			_blink = 0;

			_timer.Start();
		}

		internal void StopBlink()
		{
			_timer.Stop();

			BackColor = Color.Black;

			if (_accent_color == ColorFate) // FATE였다면
			{
				_accent_color = Color.Black;
				lblInfo.Text = string.Empty;
			}
		}

		// 모든 이벤트 없앰
		internal void EventNone()
		{
			this.Invoke((MethodInvoker)(() =>
			{
				_accent_color = Color.Black;
				lblInfo.Text = string.Empty;

				StopBlink();
			}));
		}

		// 페이트 알림
		internal void EventFate(GameData.Fate fate)
		{
			this.Invoke((MethodInvoker)(() =>
			{
				lblInfo.Text = fate.Name;

				_accent_color = ColorFate;
				StartBlink();
			}));
		}

		// 듀티 큐 상태
		internal void EventStatus(int queue)
		{
			string msg = queue < 0 ? string.Empty : $"#{queue}";

			this.Invoke((MethodInvoker)(() =>
			{
				lblInfo.Text = Mesg.GetText("ov-duties-wait", msg); ;
			}));
		}

		// 듀티 큐(루렛)
		internal void EventQueue(string name)
		{
			this.Invoke((MethodInvoker)(() =>
			{
				_accent_color = Color.Black;
				lblInfo.Text = name;
			}));
		}

		// 매치 이름(인스턴스/루렛) 표시
		internal void EventMatch(string name, bool blink = true)
		{
			this.Invoke((MethodInvoker)(() =>
			{
				lblInfo.Text = name;
				_accent_color = ColorMatch;
				if (blink)
					StartBlink();
			}));
		}
	}
}
