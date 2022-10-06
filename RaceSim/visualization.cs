using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            compas = Compas.S;
            CurserPosY = 5;
            CurserPosY = 5;

            foreach (Section section in track.Sections)
            {

                DrawSection(section.Sectiontype);
            }
        }
        public static void DrawSection(SectionTypes sectionType)
        {

            ConsoleWriteSection(SectionTypeToStringArray(sectionType));
            ChangeCurserPos();
        }
        public static string[] SectionTypeToStringArray(SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case SectionTypes.Straight:
                    return SectionTypeToDirectionalSectionType(_straightVertical, _straightHorizontal, _straightVertical, _straightHorizontal);
                case SectionTypes.LeftCorner:
                    string[] tempL = SectionTypeToDirectionalSectionType(_cornerLeftN, _cornerLeftE, _cornerLeftS, _cornerLeftW);
                    ChangeDirection(Direction.Left); ChangeCurserPosCorner(Direction.Left);
                    return tempL;
                case SectionTypes.RightCorner:
                    string[] tempR = SectionTypeToDirectionalSectionType(_cornerRightN, _cornerRightE, _cornerRightS, _cornerRightW);
                    ChangeDirection(Direction.Right); ChangeCurserPosCorner(Direction.Right);
                    return tempR;
                case SectionTypes.StartGrid:
                    return SectionTypeToDirectionalSectionType(_startVertical, _startHorizontal, _startHorizontal, _startVertical);
                case SectionTypes.Finish:
                    return SectionTypeToDirectionalSectionType(_finishVertical, _finishHorizontal, _finishHorizontal, _finishVertical);
                default:
                    return new string[0]; 
            }
        }
        public static string[] SectionTypeToDirectionalSectionType(string[] sectionN, string[] sectionE, string[] sectionS, string[] sectionW)
        {
            switch (compas)
            {
                case Compas.N:
                    return sectionN;
                case Compas.E:
                    return sectionE;
                case Compas.S:
                    return sectionS;
                case Compas.W:
                    return sectionW;
                default:
                    return new string[4];
            };
        }
        public static void ConsoleWriteSection(string[] section)
        {
            int tempY = CurserPosY;
            foreach (string s in section)
            {
                Console.Write(s);
                Console.SetCursorPosition(CurserPosX, tempY);
                tempY++;
            }
            try { Console.SetCursorPosition(CurserPosX, CurserPosY); }
            catch (ArgumentOutOfRangeException) { Console.WriteLine("Curser set out of bounds thrown"); }
        }
        public static void ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    switch (compas)
                    {
                        case Compas.N:
                            compas = Compas.W;
                            break;
                        case Compas.E:
                        case Compas.S:
                        case Compas.W:
                            compas--;
                            break;

                    }
                    break;
                case Direction.Right:
                    switch (compas)
                    {
                        case Compas.N:
                        case Compas.E:
                        case Compas.S:
                            compas++;
                            break;
                        case Compas.W:
                            compas = Compas.N;
                            break;
                    }
                    break;
            }
        }
        public static void ChangeCurserPos()
        {
            switch (compas)
            {
                case Compas.N:
                    CurserPosX -= 4;
                    break;
                case Compas.E:
                    CurserPosY += 4;
                    break;
                case Compas.S:
                    CurserPosX += 4;
                    break;
                case Compas.W:
                    CurserPosY -= 4;
                    break;

            }
        }
        public static void ChangeCurserPosCorner(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    switch (compas)
                    {
                        case Compas.N:
                            CurserPosY += 4; break;
                        case Compas.E:
                            CurserPosX -= 4; break;
                        case Compas.S:
                            CurserPosY -= 4; break;
                        case Compas.W:
                            CurserPosX += 4; break;
                    }
                    break;
                case Direction.Right:
                    switch (compas)
                    {
                        case Compas.N:
                            CurserPosY -= 4; break;
                        case Compas.E:
                            CurserPosX += 4; break;
                        case Compas.S:
                            CurserPosY += 4; break;
                        case Compas.W:
                            CurserPosX -= 4; break;
                    }
                    break;
            }
        }
    }

    public enum Direction
    {
        Left,   Right
    }
    public enum Compas
    {
        N,  E, 
                S,
            W       

        
    }
}
