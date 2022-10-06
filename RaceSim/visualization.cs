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
        //this property is just for changing the cursor pos after section has been drawn
        private static Direction DirGoing;

        #region graphics 

        public static string[] _startHorizontal = { "-----", " #   ", "     ", "   # ", "-----" };
        public static string[] _startVertical = { "|   |", "|#  |", "|   |", "|  #|", "|   |" };

        private static string[] _finishHorizontal = { "-----", "▀▄▀▄▀", "▀▄▀▄▀", "▀▄▀▄▀", "-----" };
        private static string[] _finishVertical = { "|▀▄▀|", "|▀▄▀|", "|▀▄▀|", "|▀▄▀|", "|▀▄▀|" };

        public static string[] _straightHorizontal = { "-----", "     ", "     ", "     ", "-----" };
        public static string[] _straightVertical = { "|   |", "|   |", "|   |", "|   |", "|   |" };

        //The direction (N, E, S, W) means the direction the track was facing before the corner. So a cornerLeftN wil end up with a track facing West
        public static string[] _cornerLeftN = { "-----", "   \\|", "    |", "    |", "*   |" };
        public static string[] _cornerLeftE = { "*   |", "    |", "    |", "   /|", "-----" };
        public static string[] _cornerLeftS = { "|   *", "|    ", "|    ", "|\\   ", "-----" };
        public static string[] _cornerLeftW = { "-----", "|/   ", "|    ", "|    ", "|   *" };

        public static string[] _cornerRightN = { "-----", "|/   ", "|    ", "|    ", "|   *" };
        public static string[] _cornerRightE = { "-----", "   \\|", "    |", "    |", "*   |" };
        public static string[] _cornerRightS = { "*   |", "    |", "    |", "   /|", "-----" };
        public static string[] _cornerRightW = { "|   *", "|    ", "|    ", "|\\   ", "-----" };

        #endregion
        public static void Initialize()
        {

        }
        public static void DrawTrack(Track track)
        {
            compas = Compas.E;
            DirGoing = Direction.Straight;
            CurserPosX = 24;
            CurserPosY = 16;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(CurserPosX, CurserPosY);

            foreach (Section section in track.Sections)
            {

                DrawSection(section.Sectiontype);
            }
            Console.SetCursorPosition(1, 1);
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
                     DirGoing = Direction.Left; 
                    return tempL;
                case SectionTypes.RightCorner:
                    string[] tempR = SectionTypeToDirectionalSectionType(_cornerRightN, _cornerRightE, _cornerRightS, _cornerRightW);
                    DirGoing = Direction.Right; 
                    return tempR;
                case SectionTypes.StartGrid:
                    return SectionTypeToDirectionalSectionType(_startVertical, _startHorizontal, _startVertical, _startHorizontal);
                case SectionTypes.Finish:
                    return SectionTypeToDirectionalSectionType(_finishVertical, _finishHorizontal, _finishVertical, _finishHorizontal );
                default:
                    throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
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
                    throw new ArgumentOutOfRangeException(nameof(compas), compas, null);
            };
        }
        public static void ConsoleWriteSection(string[] section)
        {
            int tempY = CurserPosY;
            Console.SetCursorPosition(CurserPosX, CurserPosY);
            foreach (string s in section)
            {
                Console.Write(s);
                tempY++;
                Console.SetCursorPosition(CurserPosX, tempY);

            }
            Console.SetCursorPosition(CurserPosX, CurserPosY);
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
            if (DirGoing != Direction.Straight)
            {
                ChangeCurserPosCorner(DirGoing);
                ChangeDirection(DirGoing);
                DirGoing = Direction.Straight;
            }
            else
            {
                switch (compas)
                {
                    case Compas.N:
                        CurserPosY -= _straightHorizontal.Length;
                        break;
                    case Compas.E:
                        CurserPosX += _straightHorizontal.Length;
                        break;
                    case Compas.S:
                        CurserPosY += _straightHorizontal.Length;
                        break;
                    case Compas.W:
                        CurserPosX -= _straightHorizontal.Length;
                        break;

                }
            }

        }
        public static void ChangeCurserPosCorner(Direction dirGoing)
        {

                switch (dirGoing)
                {
                    case Direction.Left:
                        switch (compas)
                        {
                            case Compas.N:
                                CurserPosX -= _straightHorizontal.Length; break;
                            case Compas.E:
                                CurserPosY -= _straightHorizontal.Length; break;
                            case Compas.S:
                                CurserPosX += _straightHorizontal.Length; break;
                            case Compas.W:
                                CurserPosY += _straightHorizontal.Length; break;
                        }
                        break;
                    case Direction.Right:
                        switch (compas)
                        {
                            case Compas.N:
                                CurserPosX += _straightHorizontal.Length; break;
                            case Compas.E:
                                CurserPosY += _straightHorizontal.Length; break;
                            case Compas.S:
                                CurserPosX -= _straightHorizontal.Length; break;
                            case Compas.W:
                                CurserPosY -= _straightHorizontal.Length; break;
                        }
                        break;
                    case Direction.Straight:
                        break;
                }
           
            
        }
    }

    public enum Direction
    {
        Left,   Right, Straight
    }
    public enum Compas
    {
        N,  E, 
                S,
            W       

        
    }
}
