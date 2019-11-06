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
        private const int WS_EX_TRANSPARENT = 0x20;

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        //
        private const int BlinkTime = 300;
        private const int BlinkCount = 20;

        private static readonly Color ColorFate = Color.DarkOrange;
        private static readonly Color ColorMatch = Color.Red;

        //
        private Timer _timer;
        private int _blink;
        private Color _accent_color;

        private string _current = string.Empty;
        private byte[] _members = null;

        //
        public OverlayForm()
        {
            InitializeComponent();

            Location = Settings.OverlayLocation;

            _timer = new Timer();
            _timer.Interval = BlinkTime;
            _timer.Tick += _Timer_Tick;
        }

        private void _Timer_Tick(object sender, EventArgs e)
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
                var cp = base.CreateParams;
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
            lblInfo.Text = Localization.GetText(localtext);
        }

        private void StartBlink()
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

        internal void EventNone()
        {
            _current = string.Empty;

            this.Invoke((MethodInvoker)(() =>
            {
                _accent_color = Color.Black;
                lblInfo.Text = string.Empty;

                StopBlink();
            }));
        }

        internal void EventFate(GameData.Fate fate)
        {
            _current = fate.Name;

            this.Invoke((MethodInvoker)(() =>
            {
                lblInfo.Text = _current;

                _accent_color = ColorFate;
                StartBlink();
            }));
        }

        internal void EventStatus(GameData.Instance instance, byte tank, byte healer, byte dps)
        {
            _current = instance.Name;

            if (_members == null)
                _members = new byte[] { instance.Tank, instance.Healer, instance.Dps };

            this.Invoke((MethodInvoker)(() =>
            {
                lblInfo.Text = $"{_current} ({tank}/{healer}/{dps})";
            }));
        }

        public void SetMembers(byte tank, byte healer, byte dps)
        {
            _members = new byte[] { tank, healer, dps };
        }

        internal void EventMatch(GameData.Instance instance)
        {
            _current = instance.Name;

            this.Invoke((MethodInvoker)(() =>
            {
                lblInfo.Text = _current;
                _accent_color = ColorMatch;
                StartBlink();
            }));
        }

        internal void EventRoulette(GameData.Roulette roulette)
        {
            _members = null;
            _current = roulette.Name;

            this.Invoke((MethodInvoker)(() =>
            {
                lblInfo.Text = _current;
            }));
        }

        internal void EventDuties(int count)
        {
            var msg = count < 0 ? string.Empty : $"#{count}";

            _current = Localization.GetText("ov-duties-wait", msg);

            this.Invoke((MethodInvoker)(() =>
            {
                lblInfo.Text = _current;
            }));
        }

        internal void EventDuties(int tank, int maxtank, int healer, int maxhealer, int dps, int maxdps)
        {
            var merge = $@"{tank}/{maxtank} {healer}/{maxhealer} {dps}/{maxdps}";

            _current = Localization.GetText("ov-duties-wait", merge);

            this.Invoke((MethodInvoker)(() =>
            {
                lblInfo.Text = _current;
            }));
        }
    }
}
