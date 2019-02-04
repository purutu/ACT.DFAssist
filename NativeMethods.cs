using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ACT.DFAssist
{
    public static class NativeMethods
    {
        [DllImport("Iphlpapi.dll", SetLastError = true)]
        public static extern uint GetExtendedTcpTable(IntPtr tcpTable, ref int tcpTableLength, bool sort, AddressFamily ipVersion, int tcpTableType, int reserved);

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
}
