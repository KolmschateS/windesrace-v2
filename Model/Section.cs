using System;
using System.Collections.Specialized;

namespace Model
{
    public class Section
    {
        public SectionTypes SectionType { get; set; }
        public int X, Y, Direction;
        public static readonly int SectionLength = 1000;

        public Section(SectionTypes sectionType, int x, int y, int direction)
        {
            X = x;
            Y = y;
            Direction = direction;
            SectionType = sectionType;
        }
    }
}
