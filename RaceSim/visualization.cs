using Model;
using Controller;
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
        private static Direction DirGoing;

        #region graphics 

        public static string[] _startHorizontal = { "-----", " # 1 ", "     ", " 2 # ", "-----" };
        public static string[] _startVertical = { "|   |", "|# 1|", "|   |", "|2 #|", "|   |" };

        private static string[] _finishHorizontal = { "-----", "▀▄1▄▀", "▀▄▀▄▀", "▀▄2▄▀", "-----" };
        private static string[] _finishVertical = { "|▀▄▀|", "|▀▄▀|", "|▀▄▀|", "|▀▄▀|", "|▀▄▀|" };

        public static string[] _straightHorizontal = { "-----", "   1 ", "     ", "   2 ", "-----" };
        public static string[] _straightVertical = { "|   |", "|  1|", "|   |", "|2  |", "|   |" };

        //The direction (N, E, S, W) means the direction the track was facing before the corner. So a cornerLeftN wil end up with a track facing West
        public static string[] _cornerLeftN = { "-----", "   \\|", "  1 |", " 2  |", "*   |" };
        public static string[] _cornerLeftE = { "*   |", " 1  |", "  2 |", "   /|", "-----" };
        public static string[] _cornerLeftS = { "|   *", "|  1 ", "| 2  ", "|\\   ", "-----" };
        public static string[] _cornerLeftW = { "-----", "|/   ", "| 2  ", "|  1 ", "|   *" };

        public static string[] _cornerRightN = { "-----", "|/   ", "| 2  ", "|  1 ", "|   *" };
        public static string[] _cornerRightE = { "-----", "   \\|", "   1|", " 2  |", "*   |" };
        public static string[] _cornerRightS = { "*   |", " 1  |", "   2|", "   /|", "-----" };
        public static string[] _cornerRightW = { "|   *", "|  1 ", "| 2  ", "|\\   ", "-----" };

        #endregion
        public static void Initialize()
        {
            compas = Compas.E;
            DirGoing = Direction.Straight;
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Race.NextRaceEvent += OnNextRace;
        }
        public static void DrawTrack(Track track)
        {
            CurserPosX = 55;
            CurserPosY = 3;
            Console.SetCursorPosition(CurserPosX, CurserPosY);

            foreach (Section section in track.Sections)
            {

                DrawSection(section);
            }
            Console.SetCursorPosition(1, 1);
        }
        public static void DrawSection(Section section)
        {
            ConsoleWriteSection(SectionTypeToStringArray(section.Sectiontype), section);
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
                    return SectionTypeToDirectionalSectionType(_finishVertical, _finishHorizontal, _finishVertical, _finishHorizontal);
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
        public static void ConsoleWriteSection(string[] sectionString, Section section)
        {
            int tempY = CurserPosY;
            Boolean isFinish = (section.Sectiontype == SectionTypes.Finish);
            Console.SetCursorPosition(CurserPosX, CurserPosY);
            foreach (string s in sectionString)
            {
                Console.Write(DrawParticipants(s, Data.CurrentRace.GetSectionData(section).Left, Data.CurrentRace.GetSectionData(section).Right, isFinish));
                tempY++;
                Console.SetCursorPosition(CurserPosX, tempY);

            }
            Console.SetCursorPosition(CurserPosX, CurserPosY);
        }
        public static string DrawParticipants(string s, IParticipant P1, IParticipant P2, Boolean isFinish)
        {
            //Replace the placeholders for the participants
            if (isFinish)
            {
                return s.Replace("1", P1?.Name?.Substring(0, 1) ?? "▀").Replace("2", P2?.Name?.Substring(0, 1) ?? "▀");
            }
            else
            {
                return s.Replace("1", P1?.Name?.Substring(0, 1) ?? " ").Replace("2", P2?.Name?.Substring(0, 1) ?? " ");
            }
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
        private static void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            DrawTrack(e.Track);
        }
        private static void OnNextRace(object sender, EventArgs e)
        {
            Console.Clear();
            Data.NextRace();
            if (Data.CurrentRace != null)
            {
                Initialize();
                DrawTrack(Data.CurrentRace.Track);
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
