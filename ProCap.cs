using System.Diagnostics;

namespace ACT.DFAssist
{
    class ProCap
    {
        public Process Process { get; }
        public Network.Capture Capture { get; }

        public ProCap(Process p, Network.Capture c)
        {
            Process = p;
            Capture = c;
        }
    }
}
