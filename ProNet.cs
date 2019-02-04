using System.Diagnostics;

namespace ACT.DFAssist
{
    class ProNet
    {
        public Process Process { get; }
        public Network Network { get; }

        public ProNet(Process p, Network n)
        {
            Process = p;
            Network = n;
        }
    }
}
