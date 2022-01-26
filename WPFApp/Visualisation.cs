using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace WPFApp
{
    public static class Visualisation
    {

        private static Race _race;

        #region Sizes

        internal const int SectionDimensions = 128;
        private const int ParticipantWidth = 35;
        private const int ParticipantHeight = 28;

        private const int SectionPaddingInside = 25;

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
            (int x, int y) size = GetTrackDimensions(track.Sections);
            Bitmap canvas = ImageCache.GetCanvas(size.x, size.y);
            Graphics g = Graphics.FromImage(canvas);

            foreach (Section section in track.Sections)
            {
                DrawSection(g, section);
            }
            foreach (Section section in track.Sections)
            {
                DrawParticipants(g, section);
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(canvas);
        }

        #region DrawSection

        public static void DrawSection(Graphics g, Section section)
        {
            Bitmap bmp = ImageCache.GetBitmap(DetermineSection(section));
            Bitmap rotated = RotateSection((Bitmap)bmp.Clone(), section);
            Bitmap resize = new Bitmap(rotated, new Size(SectionDimensions, SectionDimensions));
            int x = section.X * SectionDimensions / 4;
            int y = section.Y * SectionDimensions / 4;
            g.DrawImage(resize, x, y);
        }

        // TODO write test
        private static (int width, int heigth) GetTrackDimensions(LinkedList<Section> sections)
        {
            (int minX, int minY, int maxX, int maxY) dimensions = (0, 0, 0, 0);

            foreach (Section section in sections)
            {
                dimensions.minX = section.X < dimensions.minX ? section.X : dimensions.minX;
                dimensions.maxX = section.X > dimensions.maxX ? section.X : dimensions.maxX;
                dimensions.minY = section.Y < dimensions.minY ? section.Y : dimensions.minY;
                dimensions.maxY = section.Y > dimensions.maxY ? section.Y : dimensions.maxY;
            }
            int width = (dimensions.maxX + Math.Abs(dimensions.minX)) * SectionDimensions / 4 + SectionDimensions;
            int height = (dimensions.maxY + Math.Abs(dimensions.minY)) * SectionDimensions / 4 + SectionDimensions;

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
                Bitmap resize = new Bitmap(bmp, new Size(40, 32));
                Bitmap rotatedP = RotateParticipant(resize, section, distance);
                if (IsSectionACorner(section.SectionType))
                {
                    int sX = CalculateCoordinateBasedOnConsoleCoordinates(section.X);
                    int sY = CalculateCoordinateBasedOnConsoleCoordinates(section.Y);
                    (int x, int y) cornerCoords = DetermineParticipantCoordinatesCircle( CalculateAngle(section.SectionType, distance), GetRadius(isLeft));
                    coord = ReverseCoordsBasedOnDirectionAndSection((sX, sY), cornerCoords, section.SectionType, section.Direction);
                }
                else
                {
                    coord = DetermineParticipantCoordinatesStraight(isLeft, section, distance);
                }
                g.DrawImage(rotatedP, coord.x, coord.y);
                if (p.Equipment.IsBroken) { g.DrawImage(ImageCache.GetBitmap(Broken), coord.x, coord.y); }
            }
        }
        private static (int x, int y) DetermineParticipantCoordinatesStraight(bool isLeft, Section section, int distance)
        {
            (int x, int y) result = (CalculateCoordinateBasedOnConsoleCoordinates(section.X), CalculateCoordinateBasedOnConsoleCoordinates(section.Y));
            switch (section.Direction)
            {
                case 0:
                    result.x += GetLRPositionOnSection(isLeft, section.Direction);
                    result.y += DistanceInPixels(Section.SectionLength) - DistanceInPixels(distance);
                    break;
                case 1:
                    result.x += DistanceInPixels(distance);
                    result.y += GetLRPositionOnSection(isLeft, section.Direction);
                    break;
                case 2:
                    result.x += GetLRPositionOnSection(isLeft, section.Direction);
                    result.y += DistanceInPixels(distance);
                    break;
                case 3:
                    result.x += DistanceInPixels(Section.SectionLength) - DistanceInPixels(distance);
                    result.y += GetLRPositionOnSection(isLeft, section.Direction);
                    break;
                default:
                    throw (new Exception($"Direction out of bounds in DetermineParticipantCoordinates: Direction: {section.Direction}"));
            }
            result.x -= ParticipantWidth / 2;
            result.y -= ParticipantHeight / 2;
            return result;
        }

        public static (int x, int y) DetermineParticipantCoordinatesCircle(int angle, int radius)
        {
            int x = (int)(radius * Math.Sin(Math.PI * angle / 180));
            int y = (int)(radius * Math.Cos(Math.PI * angle / 180));

            return (x,y);
        }

        public static int CalculateAngle(SectionTypes st, int distance)
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
                g.RotateTransform(CalculateRotationAngle(section.Direction, section.SectionType, distance));
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

        public static int CalculateRotationAngle(int direction, SectionTypes sectionType, int distance)
        {
            int circleAngle = distance / (Section.SectionLength / 90);
            int initialAngle = direction * 90;
            return sectionType == SectionTypes.LeftCorner ? initialAngle - circleAngle : initialAngle + circleAngle;
        }
        public static int DistanceInPixels(int distance)
        {
            double calc = (double)distance / Section.SectionLength * SectionDimensions;
            return (int)calc;
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
        public static int CalculateCoordinateBasedOnConsoleCoordinates(int coord)
        {
            return coord * SectionDimensions / 4;
        }
        public static bool IsSectionACorner(SectionTypes st)
        {
            return st != SectionTypes.Straight && st != SectionTypes.StartGrid && st != SectionTypes.Finish;
        }

        public static int GetRadius(bool isLeft)
        {
            return isLeft ? SectionDimensions / 2 + SectionPaddingInside : SectionDimensions / 2 - SectionPaddingInside;
        }
        public static int GetLRPositionOnSection(bool isLeft, int direction)
        {
            if (direction <= 1)
            {
                return isLeft ? SectionDimensions / 2 - SectionPaddingInside : SectionDimensions / 2 + SectionPaddingInside;
            }
            else
            {
                return isLeft ? SectionDimensions / 2 + SectionPaddingInside : SectionDimensions / 2 - SectionPaddingInside;
            }
        }

        public static (int x, int y) ReverseCoordsBasedOnDirectionAndSection((int x, int y) sectionCoords, (int x, int y) circleCoords, SectionTypes st, int dir)
        {
            (int x, int y) resultCircle = (0, 0);
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
