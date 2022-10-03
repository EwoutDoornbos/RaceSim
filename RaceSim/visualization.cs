using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceSim
{
    public static class visualization
    {
        #region graphics 

        public static string[] _startHorizontal = { "----", " #  ", "  # ", "----" };
        public static string[] _startVertical = { "-  -", "-# -", "- #-", "-  -" };

        private static string[] _finishHorizontal = { "----", "▄▀▄▀", "▄▀▄▀", "----" };
        private static string[] _finishVertical = { "-▀▄-", "-▀▄-", "-▀▄-", "-▀▄-" };

        public static string[] _straightHorizontal = { "----", "    ", "    ", "----" };
        public static string[] _straightVertical = { "-  -", "-  -", "-  -", "-  -" };

        public static string[] _cornerLeftN = { "----", "  --", "   -", "-  -" };
        public static string[] _cornerLeftE = { "-  -", "   -", "  --", "----" };
        public static string[] _cornerLeftS = { "-  -", "-   ", "--  ", "----" };
        public static string[] _cornerLeftW = { "----", "--  ", "-   ", "-  -" };

        public static string[] _cornerRightN = { "----", "--  ", "-   ", "-  -" };
        public static string[] _cornerRightE = { "-  -", "-   ", "--  ", "----" };
        public static string[] _cornerRightS = { "-  -", "   -", "  --", "----" };
        public static string[] _cornerRightW = { "----", "  --", "   -", "-  -" };

        #endregion
        public static void Initialize()
        {

        }
        public static void DrawTrack(Track track)
        {

        }
    }
}
