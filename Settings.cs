using System.Collections.Generic;

namespace ACT.DFAssist
{
    static class Settings
    {
        public static string Path { get; set; }
        public static HashSet<int> SelectedFates { get; set; } = new HashSet<int>();

        public static bool LoggingWholeFates { get; set; }

        public static string PluginPath { get; set; } = "";
    }
}
