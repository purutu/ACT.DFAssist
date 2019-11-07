using System;
using System.Diagnostics;

namespace ACT.DFAssist
{
    internal class ProNet
    {
        public Process Process { get; }
        public Network Network { get; }
        public ClientType ClientType { get; }

        public ProNet(Process p, Network n)
        {
            Process = p;
            Network = n;

            try
            {
                ClientType =
                    p.MainModule.FileName.Contains("KOREA") ? ClientType.Korea :
                    p.MainModule.FileName.Contains("CHINA") ? ClientType.China :
                    ClientType.Global;
            }
            catch (Exception)
            {
                ClientType = ClientType.Global;
            }
        }
    }
}
