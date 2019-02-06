using System.Collections.Generic;
using ACT.DFAssist.Toolkits;

namespace ACT.DFAssist
{
    static class Settings
    {
        public static string Path { get; set; }
        public static readonly ConcurrentHashSet<int> SelectedFates = new ConcurrentHashSet<int>();


        public static bool LoggingWholeFates { get; set; }

        public static string PluginPath { get; set; } = "";
    }
}
