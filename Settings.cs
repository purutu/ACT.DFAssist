using System.Collections.Generic;
using System.Drawing;
using ACT.DFAssist.Toolkits;

namespace ACT.DFAssist
{
    static class Settings
    {
        public static string Path { get; set; }
        public static readonly ConcurrentHashSet<string> SelectedFates = new ConcurrentHashSet<string>();

        public static bool LoggingWholeFates { get; set; }
        public static bool UseOverlay { get; set; }
        public static Point OverlayLocation { get; set; } = new Point(0, 0);

        public static string PluginPath { get; set; } = "";

        public static bool AutoHideOverlay { get; set; }
    }
}
