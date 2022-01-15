using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media.Imaging;

namespace WPFApp
{
    public static class Visualisation
    {

        private static Race _race;

        #region Sizes

        internal const int SectionDimensions = 168;
        private const int ParticipantWidth = 40;
        private const int ParticipantHeight = 32;

        private const int SectionPaddingInside = 25;
        private const int SectionPaddingOutside = 40;

        #endregion

        #region Images
        // TODO make these build ready with a dynamic path
        // Sections
        private const string Straight = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Section\Straight.png";
        private const string Finish = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Section\Finish.png";
        private const string Start = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Section\Start.png";
        private const string Corner = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Section\Corner.png";

        // Spacecrafts
        private const string BlueSC = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Spacecraft\BlueSC.png";
        private const string GreenSC = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Spacecraft\GreenSC.png";
        private const string GreySC = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Spacecraft\GreySC.png";
        private const string RedSC = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Spacecraft\RedSC.png";
        private const string YellowSC = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Spacecraft\YellowSC.png";
        private const string Broken = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\Spacecraft\Broken.png";

        public static void Initialize(Race race)
        {
            _race = race;
        }

        #endregion
        public static BitmapSource DrawTrack(Track track)
        {
            (int x, int y) size = GetTrackDimensions(track);
            Bitmap canvas = ImageCache.GetCanvas(size.x, size.y);
            Graphics g = Graphics.FromImage(canvas);

            foreach(Section section in track.Sections)
            {
                DrawSection(g, section);
                DrawParticipants(g, section);
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(canvas);
        }

        #region DrawSection

        public static void DrawSection(Graphics g, Section section)
        {
            Bitmap bmp = ImageCache.GetBitmap(DetermineSection(section));
            Bitmap rotated = RotateSection((Bitmap)bmp.Clone(), section);
            int x = section.X * SectionDimensions / 4;
            int y = section.Y * SectionDimensions / 4;
            g.DrawImage(rotated, x, y);
        }


        private static (int width, int heigth) GetTrackDimensions(Track track)
        {
            (int minX, int minY, int maxX, int maxY) d = (0, 0, 0, 0);

            foreach (Section section in track.Sections)
            {
                if (section.X < d.minX) d.minX = section.X;
                if (section.X > d.maxX) d.maxX = section.X;
                if (section.Y < d.minY) d.minY = section.Y;
                if (section.Y > d.maxY) d.maxY = section.Y;
            }
            int width = (d.maxX - Math.Abs(d.minX)) * SectionDimensions / 4 + SectionDimensions;
            int height = (d.maxY - Math.Abs(d.minY)) * SectionDimensions / 4 + SectionDimensions;

            return (width, height);
        }

        private static string DetermineSection(Section section)
        {
            if (section.SectionType == SectionTypes.Straight) return Straight;
            if (section.SectionType == SectionTypes.Finish) return Finish;
            if (section.SectionType == SectionTypes.StartGrid) return Start;
            if (section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner) return Corner;
            return Straight;
        }

        #region SectionRotation
        private static Bitmap RotateSection(Bitmap bitmap, Section section)
        {
            if (section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner)
            {
                return RotateCornerSection(bitmap, section);
            }
            else
            {
                return RotateStraightSection(bitmap, section);
            }
        }

        private static Bitmap RotateCornerSection(Bitmap bitmap, Section section)
        {
            if (section.SectionType == SectionTypes.LeftCorner)
            {
                switch (section.Direction)
                {
                    case 1:
                        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 2:
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 3:
                        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                        break;
                }
            }
            if (section.SectionType == SectionTypes.RightCorner)
            {
                switch (section.Direction)
                {
                    case 1:
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                        break;
                    case 2:
                        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 3:
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    default:
                        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
            }
            return bitmap;
        }

        private static Bitmap RotateStraightSection(Bitmap bitmap, Section section)
        {
            switch (section.Direction)
            {
                case 1:
                    bitmap.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
                case 2:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 3:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                default:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            return bitmap;
        }

        #endregion
        #endregion

        #region DrawParticipants
        public static void DrawParticipants(Graphics g, Section section)
        {
            int x = CalculateCoordinateBasedOnConsoleCoordinates(section.X);
            int y = CalculateCoordinateBasedOnConsoleCoordinates(section.Y);

            SectionData sd = _race.GetSectionData(section);
            DrawParticipant(g, sd.Left, sd.DistanceLeft, section, true);
            DrawParticipant(g, sd.Right, sd.DistanceRight, section, false);
        }

        private static void DrawParticipant(Graphics g, IParticipant p, int distance, Section section, bool isLeft)
        {
            if (p != null)
            {
                (int x, int y) coord = (0, 0);
                Bitmap bmp = new Bitmap(GetImageBasedOnTeamColor(p.TeamColor));
                Bitmap resize = new Bitmap(bmp, new Size(40 ,32));
                Bitmap rotatedP = RotateParticipant(resize, section, distance);
                if (IsSectionACorner(section.SectionType))
                {
                    coord = DetermineParticipantCoordinatesCircle(p, isLeft, section, distance);
                }
                else
                {
                    coord = DetermineParticipantCoordinatesStraight(p, isLeft, section, distance);
                }
                //g.DrawEllipse(new Pen(new SolidBrush(Color.White)),coord.x, coord.y, 5, 5);
                g.DrawImage(rotatedP, coord.x, coord.y);
                if (p.Equipment.IsBroken) { g.DrawImage(ImageCache.GetBitmap(Broken), coord.x, coord.y); }
            }
        }
        private static (int x, int y) DetermineParticipantCoordinatesStraight(IParticipant p, bool isLeft, Section section, int distance)
        {
            (int x, int y) result = (CalculateCoordinateBasedOnConsoleCoordinates(section.X), CalculateCoordinateBasedOnConsoleCoordinates(section.Y));
            switch (section.Direction)
            {
                case 0:
                    result.x += GetLRPositionOnSection(isLeft);
                    result.y += DistanceInPixels(Section.SectionLength) - DistanceInPixels(distance);
                    break;
                case 1:
                    result.x += DistanceInPixels(distance);
                    result.y += GetLRPositionOnSection(isLeft);
                    break;
                case 2:
                    result.x += GetLRPositionOnSection(isLeft);
                    result.y += DistanceInPixels(distance);
                    break;
                case 3:
                    result.x += DistanceInPixels(Section.SectionLength) - DistanceInPixels(distance);
                    result.y += GetLRPositionOnSection(isLeft);
                    break;
                default:
                    throw(new Exception($"Direction out of bounds in DetermineParticipantCoordinates: Direction: {section.Direction}"));
            }
            result.x -= ParticipantWidth / 2;
            //result.y -= ParticipantHeight / 2;
            return result;
        }

        // TODO makes function that calculates where the participants needs to be drawn while in a corner.
        private static (int x, int y) DetermineParticipantCoordinatesCircle(IParticipant p, bool isLeft, Section section, int distance)
        {
            (int x, int y) sectionCoords = (CalculateCoordinateBasedOnConsoleCoordinates(section.X), CalculateCoordinateBasedOnConsoleCoordinates(section.Y));


            int angle = CalculateAngle(section.SectionType, distance);

            int radius = GetRadius(section.SectionType, isLeft);

            int x = (int)(radius * Math.Sin(Math.PI * 2 * angle / 360));
            int y = (int)(radius * Math.Cos(Math.PI * 2 * angle / 360));

            return ReverseCoordsBasedOnDirectionAndSection(sectionCoords, (x, y), section.SectionType, section.Direction);
        }
        private static int CalculateAngle(SectionTypes st, int distance)
        {
            return st == SectionTypes.LeftCorner ? distance / (Section.SectionLength / 90) : 90 - (distance / (Section.SectionLength / 90));
        }
        private static Bitmap RotateParticipant(Bitmap p, Section section, int distance)
        {

            Bitmap returnBitmap = new Bitmap(p.Width, p.Height);

            Graphics g = Graphics.FromImage(returnBitmap);

            //move rotation point to center of image
            g.TranslateTransform((float)p.Width / 2, (float)p.Height / 2);
            //rotate
            if (IsSectionACorner(section.SectionType))
            {
                g.RotateTransform(DetermineParticipantRotationInCorner(section.SectionType, section.Direction, distance));
            }
            else
            {
                g.RotateTransform(section.Direction * 90);
                //move image back
            }
            g.TranslateTransform(-(float)p.Width / 2, -(float)p.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(p, new Point(0, 0));

            return returnBitmap;
        }

        private static int DistanceInPixels(int distance)
        {
            return distance / (Section.SectionLength / SectionDimensions);
        }

        private static string GetImageBasedOnTeamColor(TeamColors tc)
        {
            switch (tc)
            {
                case TeamColors.Red:
                    return RedSC;
                case TeamColors.Green:
                    return GreenSC;
                case TeamColors.Yellow:
                    return YellowSC;
                case TeamColors.Grey:
                    return GreySC;
                case TeamColors.Blue:
                    return BlueSC;
                default:
                    throw new Exception($"Incorrect TeamColor entered in GetImageBasedOnTeamColor {tc}");
            }
        }
        #endregion
        private static int CalculateCoordinateBasedOnConsoleCoordinates(int coord)
        {
            return coord * SectionDimensions / 4;
        }
        private static bool IsSectionACorner(SectionTypes st)
        {
            return st != SectionTypes.Straight && st != SectionTypes.StartGrid && st != SectionTypes.Finish;
        }
        private static int GetRadius(SectionTypes st, bool isLeft)
        {
            // Checks if the corner goes to right and the position is at the inside
            bool IsInnerRadius = (st == SectionTypes.LeftCorner && isLeft) || (st == SectionTypes.RightCorner && !isLeft);

            // If the corner is on the inside return the outerPadding otherwise return the position to the middle
            return GetLRPositionOnSection(IsInnerRadius);
        }

        private static int GetLRPositionOnSection(bool isLeft)
        {
            int middle = SectionDimensions / 2;
            return isLeft ? middle + SectionPaddingInside : middle - SectionPaddingInside;
        }

        private static int DetermineParticipantRotationInCorner(SectionTypes st, int dir, int distance)
        {
            if ((st == SectionTypes.RightCorner && dir == 0) || (st == SectionTypes.LeftCorner && dir == 1)){ return 45; }

            if ((st == SectionTypes.RightCorner && dir == 1) || (st == SectionTypes.LeftCorner && dir == 2)) { return 90 + 45; }

            if ((st == SectionTypes.RightCorner && dir == 2) || (st == SectionTypes.LeftCorner && dir == 3)) { return 180 + 45; }

            if ((st == SectionTypes.RightCorner && dir == 3) || (st == SectionTypes.LeftCorner && dir == 0)) { return 270 + 45; }
            throw new Exception($"Wrong sectiontype or direction entered in DetermineParticipantRotationInCorner sectiontype:{st} direction:{dir}");
        }
        private static (int x, int y) ReverseCoordsBasedOnDirectionAndSection((int x, int y) sectionCoords,(int x, int y) circleCoords, SectionTypes st, int dir)
        {
            (int x, int y) resultCircle = (0,0);
            if (st == SectionTypes.LeftCorner)
            {
                switch (dir)
                {
                    case 0:
                        resultCircle.x = circleCoords.y;
                        resultCircle.y = SectionDimensions - circleCoords.x;
                        break;
                    case 1:
                        resultCircle = circleCoords;
                        break;
                    case 2:
                        resultCircle.x = SectionDimensions - circleCoords.y;
                        resultCircle.y = circleCoords.x;
                        break;
                    case 3:
                        resultCircle.x = SectionDimensions - circleCoords.x;
                        resultCircle.y = SectionDimensions - circleCoords.y;
                        break;
                    default:
                        break;
                }
            }
            else if (st == SectionTypes.RightCorner)
            {
                switch (dir)
                {
                    case 0:
                        resultCircle.x = SectionDimensions - circleCoords.x;
                        resultCircle.y = SectionDimensions - circleCoords.y;
                        break;
                    case 1:
                        resultCircle.x = circleCoords.y;
                        resultCircle.y = SectionDimensions - circleCoords.x;
                        break;
                    case 2:
                        resultCircle = circleCoords;
                        break;
                    case 3:
                        resultCircle.x = SectionDimensions - circleCoords.y;
                        resultCircle.y = circleCoords.x;
                        break;
                    default:
                        break;
                }
            }

            sectionCoords.x += resultCircle.x - (ParticipantWidth / 2);
            sectionCoords.y += resultCircle.y - (ParticipantHeight / 2);
            return sectionCoords;
        }

    }
}
