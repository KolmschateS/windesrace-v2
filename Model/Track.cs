using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public int StartDirection { get; set; }
        public int GridSize { get; set; }
        private readonly int SectionSize = 4;

        public Track(string name, SectionTypes[] sections, int startDirection)
        {
            Name = name;
            StartDirection = startDirection;
            GridSize = SetGridSize(sections);
            Sections = SetSections(sections, startDirection);
        }
        // Funtion to set the Sections property by looping over an Array containing the sections and inserting them
        // into the Linkedlist with sections
        public LinkedList<Section> SetSections(SectionTypes[] sections, int startDirection)
        {
            LinkedList<Section> sectionsList = SetSectionCoordinates(sections, startDirection);

            int[] startPositionXy = CaluclateMinimalXandY(sectionsList);
            sectionsList = RecalculateCoordinatesInSections(sectionsList, startPositionXy);

            return sectionsList;
        }

        public int[] CaluclateMinimalXandY(LinkedList<Section> sectionsList)
        {
            int[] xy = {0, 0};
            foreach (Section section in sectionsList)
            {
                if (section.X < xy[0]) xy[0] = section.X;
                if (section.Y < xy[1]) xy[1] = section.Y;
            }
            return xy;
        }

        public LinkedList<Section> SetSectionCoordinates(SectionTypes[] sections, int startDirection)
        {
            int[] currentPos = {0, 0};
            int direction = startDirection;
            LinkedList<Section> sectionsList = new LinkedList<Section>();
            
            foreach (SectionTypes section in sections)
            {
                currentPos[1] += SectionSize;
                int[] sectionPosition = ChangeCursorPos(direction);
                currentPos[0] += sectionPosition[0];
                currentPos[1] += sectionPosition[1];
                sectionsList.AddLast(new Section(section, currentPos[0], currentPos[1], direction));
                direction = ChangeDirection(section, direction);
            }
            return sectionsList;
        }
        public LinkedList<Section> RecalculateCoordinatesInSections(LinkedList<Section> sections, int[] startingPos)
        {
            foreach (Section section in sections)
            {
                section.X += Math.Abs(startingPos[0]);
                section.Y += Math.Abs(startingPos[1]);
            }

            return sections;
        }

        // For each section look if it is an startgrid. If so add the count with 2.
        public int SetGridSize(SectionTypes[] sections)
        {
            int count = 0;
            foreach (SectionTypes sectionType in sections)
                if (sectionType == SectionTypes.StartGrid) count += 2;
            return count;
        }
        // Gets the node of the Finish from the Linkedlist with Sections.
        // Used in Race.GetStartGrid to return the Startgrid of the Track
        public LinkedListNode<Section> GetFinishNodeFromSections()
        {
            LinkedListNode<Section> node = Sections.First;
            while (true)
            {
                if (node == null)
                {
                    throw new Exception("Node in GetFinishNodeFromSections is null");
                }

                if (node.Value.SectionType == SectionTypes.Finish)
                {
                    return node;
                }

                node = node.Next;
            }
        }
        // Returns the Startgrid as a list in the order of the closest startgrid to the finish
        public List<Section> GetStartgrid()
        {
            LinkedListNode<Section> node = GetFinishNodeFromSections();
            List<Section> result = new List<Section>();
            while (true)
            {
                node = node.Previous ?? Sections.Last;
                if (node.Value.SectionType == SectionTypes.Finish) break;

                if (node.Value.SectionType == SectionTypes.StartGrid) result.Add(node.Value);
            }
            return result;
        }
        // Function to change direction based on the sectionType that is given
        // Corners change the direction
        //              0
        //      3               1
        //              2
        // The switch case makes it possible to go to -1 or 4. This Wrap function makes this a zero.
        public int ChangeDirection(SectionTypes sectiontype, int direction)
        {
            switch (sectiontype)
            {
                case SectionTypes.LeftCorner:
                    direction -= 1;
                    break;

                case SectionTypes.RightCorner:
                    direction += 1;
                    break;
            }

            return Wrap(4, direction);
        }

        private int Wrap(int max, int x)
        {
            return (x + max) % max;
        }
 
        // Function to set the cursor position to draw a section accordingly
        public int[] ChangeCursorPos(int direction)
        {
            switch (direction)
            {
                // Direction is 0
                //   1234
                // 0 +           Cursor should go from 0,8 to 0,0 to draw a section with direction 0
                // 1   
                // 2                                    ^
                // 3                                    |
                // 4 ....                               |
                // 5 ....                               |
                // 6 ....
                // 7 ....
                // 8 +
                case 0: 
                    return new int[]{0, -8};
                
                // Direction is 1
                //   12345678
                // 0 ....+...      Cursor should go from 5,4 to 5,0 to draw a section with direction 0
                // 1 ........  
                // 2 ........                          ---->     
                // 3 ........
                // 4 +       
                case 1:
                    return new int[]{4, -4};
                
                // Direction is 2
                //   1234
                // 0 ....           Cursor should stay where it is
                // 1 ....                       |
                // 2 ....                       |
                // 3 ....                       |
                // 4 +                          ↓
                case 2: 
                    return new int[]{0, 0};
                
                // Direction is 3
                //   01234567
                // 0 +.......      Cursos should go from 4,4 to 0,0 to draw a section with direction 3
                // 1 ........  
                // 2 ........                       <----
                // 3 ........
                // 4     +   
                case 3:
                    return new int[]{-4, -4};
                default:
                    return new int[]{10, 10};
            }
        }

        public Section GetNextSection(Section currentSection, int stepsToTake)
        {
            LinkedListNode<Section> node = Sections.Find(currentSection);
            for (int i = 0; i < stepsToTake; i++)
            {
                if (node == null) { throw new Exception("Node in GetFinishNodeFromSections is null"); }
                node = node.Next;
            }
            if (node == null) { throw new Exception("Node in GetFinishNodeFromSections is null"); }
            
            return node.Value;
        }
    }
}
