using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RaceSim
{
    public static class visualization
    {
        private static int CurserPosX;
        private static int CurserPosY;
        
        private static Compas compas;

        #region graphics 

        public static string[] _startHorizontal = { "----", " #  ", "  # ", "----" };
        public static string[] _startVertical = { "-  -", "-# -", "- #-", "-  -" };

        private static string[] _finishHorizontal = { "----", "▄▀▄▀", "▄▀▄▀", "----" };
        private static string[] _finishVertical = { "-▀▄-", "-▀▄-", "-▀▄-", "-▀▄-" };

        public static string[] _straightHorizontal = { "----", "    ", "    ", "----" };
        public static string[] _straightVertical = { "-  -", "-  -", "-  -", "-  -" };

        //The direction (N, E, S, W) means the direction the track was facing before the corner. So a cornerLeftN wil end up with a track facing West
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
            compas = Compas.N;
            
            LinkedList<Section> sections = track.Sections;

            foreach(Section section in sections)
            {
                DrawSection(section.Sectiontype);
            }
        }
        public static void DrawSection(SectionTypes sectionType)
        {

            ConsoleWriteSection(SectionTypeToStringArray(sectionType));
        }
        public static string[] SectionTypeToStringArray(SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case SectionTypes.Straight:
                    return SectionTypeToDirectionalSectionType(_straightVertical, _straightHorizontal, _straightHorizontal, _straightVertical);
                    break;
                case SectionTypes.LeftCorner:
                    return SectionTypeToDirectionalSectionType(_cornerLeftN, _cornerLeftE, _cornerLeftS, _cornerLeftW);
                    break;
                case SectionTypes.RightCorner:
                    return SectionTypeToDirectionalSectionType(_cornerRightN, _cornerRightE, _cornerRightS, _cornerRightW);
                    break;
                case SectionTypes.StartGrid:
                    return SectionTypeToDirectionalSectionType(_startVertical, _startHorizontal, _startHorizontal, _startVertical);
                    break;
                case SectionTypes.Finish:
                    return SectionTypeToDirectionalSectionType(_finishVertical, _finishHorizontal, _finishHorizontal, _finishVertical);
                    break;
            }
            return new string[0];
        }
        public static string[] SectionTypeToDirectionalSectionType(string[] sectionN, string[] sectionE, string[] sectionS, string[] sectionW)
        {
            switch ((int)compas)
            {
                case 0:
                    return sectionN;
                    break;
                case 1:
                    return sectionE;
                    break;
                case 2:
                    return sectionS;
                    break;
                case 3:
                    return sectionW;
                    break;
                    default: throw new ArgumentOutOfRangeException(nameof(compas));
            };
        }
        public static void ConsoleWriteSection(string[] section)
        {
            foreach(string s in section)
            {
                Console.WriteLine(s);
            }
        }
    }
    public enum Compas
    {
        N,  E, S,

            W       

        
    }
}
