using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Common;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Media.Media3D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using Model;
using System.ComponentModel.Design;
using Controller;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace WpfProject
{
    public static class VisualizationWPF
    {

        #region Graphics

        private const string _start = @".\..\..\..\Assets\Tracks\StartGrid.png";
        private const string _finish = @".\..\..\..\Assets\Tracks\Finish.png";
        private const string _corner = @".\..\..\..\Assets\Tracks\Corner.png";
        private const string _straight = @".\..\..\..\Assets\Tracks\Straight.png";
        private const string _blue = @".\..\..\..\Assets\Cars\Cars\Blue.png";
        private const string _green = @".\..\..\..\Assets\Cars\Cars\Green.png";
        private const string _grey = @".\..\..\..\Assets\Cars\Cars\Grey.png";
        private const string _red = @".\..\..\..\Assets\Cars\Cars\Red.png";
        private const string _yellow = @".\..\..\..\Assets\Cars\Cars\Yellow.png";
        private const string _blueBroken = @".\..\..\..\Assets\Cars\Broken\Blue.png";
        private const string _greenBroken = @".\..\..\..\Assets\Cars\Broken\Green.png";
        private const string _greyBroken = @".\..\..\..\Assets\Cars\Broken\Grey.png";
        private const string _redBroken = @".\..\..\..\Assets\Cars\Broken\Red.png";
        private const string _yellowBroken = @".\..\..\..\Assets\Cars\Broken\Yellow.png";

        #endregion

        private static int CurserPosX;
        private static int CurserPosY;

        private static Compas compas;
        private static Direction DirGoing;

        private static Bitmap TrackCanvas;
        private static Graphics TrackGraphics;

        public static void Initialize()
        {
            compas = Compas.E;
            DirGoing = Direction.Straight;
        }

        public static BitmapSource DrawTrack(Model.Track track)
        {
            Initialize();
            CurserPosY = 0;
            CurserPosX = 256;
            (int TrackWidth, int TrackHeight) = GetTrackSize(track.Sections);
            TrackCanvas = ImagesCache.GetBitmapEmpty(TrackWidth, TrackHeight);
            TrackGraphics = Graphics.FromImage(TrackCanvas);
/*            Bitmap b = new Bitmap(SectionTypeToBitmap(SectionTypes.RightCorner));
            Graphics g = Graphics.FromImage(b);
            IParticipant P1 = Data.competition.Participants[3];
            IParticipant P2 = Data.competition.Participants[4];
            g.DrawImage(RotateBitmap(new Bitmap(P1Bitmap)), 30, 60);   
            g.DrawImage(RotateBitmap(new Bitmap(P2Bitmap)), 70, 10);   
            TrackGraphics.DrawImage(b, CurserPosX, CurserPosY);*/
            foreach (Section section in track.Sections)
            {

                DrawSection(section);
            }
            return ImagesCache.GetBitmapSource(TrackCanvas);
        }
        public static (int Width, int Height) GetTrackSize(LinkedList<Section> sections)
        {
            Compas TempCompas = Compas.E;
            int x, y, maxX, minX, maxY, minY;
            x = y = maxX = minX = maxY = minY = 0;
            foreach (Section section in sections)
            {
                switch (TempCompas)
                {
                    case Compas.N:
                        y--;
                        break;
                    case Compas.E:
                        x++;
                        break;
                    case Compas.S:
                        y++;
                        break;
                    case Compas.W:
                        x--;
                        break;
                }
                switch (section.Sectiontype)
                {
                    case SectionTypes.LeftCorner:
                        TempCompas = ChangeDirection(Direction.Left, TempCompas);
                        break;
                    case SectionTypes.RightCorner:
                        TempCompas = ChangeDirection(Direction.Right, TempCompas);
                        break;
                }
                if (x > maxX) maxX = x;
                else if (x < minX) minX = x; 
                if (y > maxY) maxY = y;
                else if (y < minY) minY = y;

            }
            Bitmap sizeCheck = ImagesCache.GetImageBitmap(_straight);
            maxX++; maxY++;
            return (((maxX - minX) * sizeCheck.Width) , ((maxY - minY) * sizeCheck.Width));
        }
        public static void DrawSection(Section section)
        {
            DrawSingleSection(SectionTypeToBitmap(section.Sectiontype), section);
            ChangeCurserPos();
        }
        public static Bitmap SectionTypeToBitmap(SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case SectionTypes.Straight:
                    return RotateBitmap(new Bitmap(ImagesCache.GetImageBitmap(_straight)));
                case SectionTypes.LeftCorner:
                    Bitmap tempL = RotateBitmap(new Bitmap(ImagesCache.GetImageBitmap(_corner)));
                    DirGoing = Direction.Left;
                    return tempL;
                case SectionTypes.RightCorner:
                    Bitmap tempr = new Bitmap(ImagesCache.GetImageBitmap(_corner)); tempr.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    Bitmap tempR = RotateBitmap(tempr);
                    DirGoing = Direction.Right;
                    return tempR;
                case SectionTypes.StartGrid:
                    return RotateBitmap(new Bitmap(ImagesCache.GetImageBitmap(_start)));
                case SectionTypes.Finish:
                    return RotateBitmap(new Bitmap(ImagesCache.GetImageBitmap(_finish)));
                default:
                    throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
            }
        }
        public static Bitmap RotateBitmap(Bitmap bitmap)
        {
            switch (compas)
            {
                case Compas.N:
                    return bitmap;
                case Compas.E:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return bitmap;
                case Compas.S:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return bitmap;
                case Compas.W:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return bitmap;
                default:
                    throw new ArgumentOutOfRangeException(nameof(compas), compas, null);
            };
        }
        public static void DrawSingleSection(Bitmap sectionBitmap, Section section)
        {

            TrackGraphics.DrawImage(DrawParticipants(sectionBitmap, Data.CurrentRace.GetSectionData(section).Left, Data.CurrentRace.GetSectionData(section).Right, section.Sectiontype), CurserPosX, CurserPosY);
        }

        public static Bitmap DrawParticipants(Bitmap sectionBitmap, IParticipant P1, IParticipant P2, SectionTypes sectionType)
        {
            
            Graphics g = Graphics.FromImage(sectionBitmap);
            if (P1 != null)
            {
                Bitmap P1Bitmap = new Bitmap(ImagesCache.GetImageBitmap(GetParticipantURL(P1)));
                switch (sectionType)
                {
                    case SectionTypes.StartGrid:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P1Bitmap), 16, 40);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P1Bitmap), 40, 16);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P1Bitmap), 61, 39);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P1Bitmap), 39, 61);
                                break;
                        }
                        break;  
                    case SectionTypes.LeftCorner:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P1Bitmap), 10, 70);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P1Bitmap), 10, 10);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P1Bitmap), 70, 10);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P1Bitmap), 70, 70);
                                break;
                        }
                        break;  
                    case SectionTypes.RightCorner:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P1Bitmap), 20, 30);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P1Bitmap), 50, 20);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P1Bitmap), 60, 50);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P1Bitmap), 30, 60);
                                break;
                        }
                        break;  
                    case SectionTypes.Straight:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P1Bitmap), 16, 38);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P1Bitmap), 38, 16);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P1Bitmap), 66, 38);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P1Bitmap), 38, 66);
                                break;
                        }
                        break;  
                    case SectionTypes.Finish:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P1Bitmap), 16, 38);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P1Bitmap), 38, 16);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P1Bitmap), 66, 38);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P1Bitmap), 38, 66);
                                break;
                        }
                        break;
                }
            }               //Dit plaatst de Participant op de juiste pixel op de section image
            if (P2 != null)
            {
                Bitmap P2Bitmap = new Bitmap(ImagesCache.GetImageBitmap(GetParticipantURL(P2)));
                switch (sectionType)
                {
                    case SectionTypes.StartGrid:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P2Bitmap), 63, 79);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P2Bitmap), 0, 63);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P2Bitmap), 15, 0);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P2Bitmap), 79, 15);
                                break;
                        }
                        break;  
                    case SectionTypes.LeftCorner:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P2Bitmap), 70, 20);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P2Bitmap), 50, 60);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P2Bitmap), 20, 50);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P2Bitmap), 40, 20);
                                break;
                        }
                        break;  
                    case SectionTypes.RightCorner:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P2Bitmap), 70, 70);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P2Bitmap), 10, 70);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P2Bitmap), 10, 10);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P2Bitmap), 70, 10);
                                break;
                        }
                        break;  
                    case SectionTypes.Straight:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P2Bitmap), 66, 38);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P2Bitmap), 38, 66);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P2Bitmap), 16, 38);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P2Bitmap), 38, 16);
                                break;
                        }
                        break;  
                    case SectionTypes.Finish:
                        switch (compas)
                        {
                            case Compas.N:
                                g.DrawImage(RotateBitmap(P2Bitmap), 66, 38);
                                break;
                            case Compas.E:
                                g.DrawImage(RotateBitmap(P2Bitmap), 38, 66);
                                break;
                            case Compas.S:
                                g.DrawImage(RotateBitmap(P2Bitmap), 16, 38);
                                break;
                            case Compas.W:
                                g.DrawImage(RotateBitmap(P2Bitmap), 38, 16);
                                break;
                        }
                        break;
                }
            }
            return sectionBitmap;            
        }
        public static string GetParticipantURL(IParticipant P)
        {
                bool broken = P.Equipement.isBroken;
                switch (P.TeamColor)
                {
                    case TeamColors.Red:
                        return broken ? _redBroken : _red;
                    case TeamColors.Blue:
                        return broken ? _blueBroken : _blue;
                    case TeamColors.Yellow:
                        return broken ? _yellowBroken : _yellow;
                    case TeamColors.Grey:
                        return broken ? _greyBroken : _grey;
                    case TeamColors.Green:
                        return broken ? _greenBroken : _green;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(P.TeamColor), P.TeamColor, null);
                }
        }
        public static Compas ChangeDirection(Direction direction, Compas compas)
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
                    return compas;
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
                    return compas;
                default:
                    return compas;
            }

        }
        public static void ChangeCurserPos()
        {
            Bitmap sizeCheck = ImagesCache.GetImageBitmap(_straight);
            if (DirGoing != Direction.Straight)
            {
                ChangeCurserPosCorner(DirGoing);
                compas = ChangeDirection(DirGoing, compas);
                DirGoing = Direction.Straight;
            }
            else
            {
                switch (compas)
                {
                    case Compas.N:
                        CurserPosY -= sizeCheck.Width;
                        break;
                    case Compas.E:
                        CurserPosX += sizeCheck.Width;
                        break;
                    case Compas.S:
                        CurserPosY += sizeCheck.Width;
                        break;
                    case Compas.W:
                        CurserPosX -= sizeCheck.Width;
                        break;

                }
            }

        }
        public static void ChangeCurserPosCorner(Direction dirGoing)
        {
            Bitmap sizeCheck = ImagesCache.GetImageBitmap(_straight);
            switch (dirGoing)
            {
                case Direction.Left:
                    switch (compas)
                    {
                        case Compas.N:
                            CurserPosX -= sizeCheck.Width; break;
                        case Compas.E:
                            CurserPosY -= sizeCheck.Width; break;
                        case Compas.S:
                            CurserPosX += sizeCheck.Width; break;
                        case Compas.W:
                            CurserPosY += sizeCheck.Width; break;
                    }
                    break;
                case Direction.Right:
                    switch (compas)
                    {
                        case Compas.N:
                            CurserPosX += sizeCheck.Width; break;
                        case Compas.E:
                            CurserPosY += sizeCheck.Width; break;
                        case Compas.S:
                            CurserPosX -= sizeCheck.Width; break;
                        case Compas.W:
                            CurserPosY -= sizeCheck.Width; break;
                    }
                    break;
                case Direction.Straight:
                    break;
            }
        }
        public enum Direction
        {
            Left, Right, Straight
        }
        public enum Compas
        {
            N, E,
            S,
            W


        }
    }
}
