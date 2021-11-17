using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Intrinsics.X86;
using Controller;
using Model;

namespace windesrace_v2
{
    public static class Visualisation
    {
        // The direction is determined by an integer. The integer goes from 0 to 3: 0, 1, 2, 3
        // The direction for integer is as following:
        // 0:   ^
        //      |
        //      |
        //      |
        // 1: ---->
        // 2:   |
        //      |
        //      |
        //      ↓
        // 3: <----
        // The default value is one, which is set in the Initialize function
        public static int Direction { get; set; }

        // The Two-key dictionary works as following:
        // Dictionary<(int direction, Sectiontypes sectiontype), String[] SectionString>
        public static Dictionary<(int, SectionTypes), string[]> Graphics { get; set; }

        public static void Initialize()
        {
            SetGraphics();
        }

        // Function to call to draw the track
        public static void DrawTrack(Track track)
        {
            try
            {
                Console.SetWindowSize(120, 60);
            }
            catch
            {
                Console.WriteLine("Can't set Windowsize");
            }

            // Gets the Direction from the currentTrack, "starting position"
            // SetStartingPos draws the track virtually and calculates from where
            // the track should be drawn for it to remain within the console windowsizes limits
            Direction = track.StartDirection;
            int [] startingPos = SetStartingPos(track);

            // Clears the console to make reset the canvas and writes the current trackname
            // 
            Console.Clear();
            Console.Write(track.Name);

            // Sets the cursor a little more to the bottom, so there is a space between
            // the trackname and the track
            Console.SetCursorPosition( startingPos[0], startingPos[1] + 5);

            // After SetStartingPos the Direction changed. Resetting this will prep
            // it for the actual drawing of the track.
            Direction = track.StartDirection;

            // For each section in the track
            foreach (Section section in track.Sections)
            {
                // The section is drawn, by getting from the Graphic dictionary
                // by inserting the current direction and the sectiontype
                DrawSection(section);

                // After drawing the direction is changed based on the sectiontype
                // (if it's a corner the direction obviously changes)
                ChangeDirection(section.SectionType);
                int[] cursorPos = ChangeCursorPos();
                Console.SetCursorPosition(Console.CursorLeft + cursorPos[0], Console.CursorTop + cursorPos[1]);
            }
        }

        // Function to call to draw a section
        public static void DrawSection(Section section)
        {
            string[] sectionstring = Graphics[(Direction, section.SectionType)];
            foreach (string row in sectionstring)
            {
                Console.Write(row);
                Console.SetCursorPosition(Console.CursorLeft - 4, Console.CursorTop + 1);
            }
        }


        // Function to change direction based on the sectionType that is given
        // Corners change it
        public static void ChangeDirection(SectionTypes sectiontype)
        {
            switch (sectiontype)
            {
                case SectionTypes.LeftCorner:
                    Direction -= 1;
                    break;
                

                case SectionTypes.RightCorner:
                    Direction += 1;
                    break;
            }
            Direction = Wrap(Direction, 4);
        }
        private static int Wrap(int x, int max)
        {
            return (x + max) % max;
        }

        // Function to set the cursor position to draw a section accordingly
        public static int[] ChangeCursorPos()
        {
            switch (Direction)
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
        
        public static int[] SetStartingPos(Track track)
        {
            int[] currentPos = {0, 0};
            int[] startingPos = { 0, 0 };

            foreach (var section in track.Sections)
            {
                // Imitating the writing of the section. Y gets + 4
                currentPos[1] += 4;
                ChangeDirection(section.SectionType);
                
                // Gets the instruction what the edit should be
                int[] change = ChangeCursorPos();

                for (int i = 0; i < currentPos.Length; i++)
                {
                    if (currentPos[i] + change[i] < startingPos[i])
                    {
                        startingPos[i] = currentPos[i] + change[i];
                    }
                    currentPos[i] += change[i];
                }
            }

            for(int i = 0; i < startingPos.Length; i++)
            {
                startingPos[i] = Math.Abs(startingPos[i]);
            }

            return startingPos;
        }
        
        // SetGraphics is the function that sets the two-key dictionary
        public static void SetGraphics()
        {
            Graphics = new Dictionary<(int, SectionTypes), string[]>();
            
            // Direction = 0
            // Finish vertical
            // +  +
            // +##+
            // +  +
            // +  +
            string[] finish0Vertical = { "║  ║", "║▒▒║", "║  ║", "║  ║"};
            Graphics.Add((0, SectionTypes.Finish), finish0Vertical);

            // Direction = 1
            // Finish horizontal
            // ++++
            //   #
            //   #
            // ++++
            string[] finish1Horizontal = { "════", "  ▒ ", "  ▒ ", "════" };
            Graphics.Add((1, SectionTypes.Finish), finish1Horizontal);

            // Direction = 2
            // Finish vertical
            // +  +
            // +  +
            // +##+
            // +  +
            string[] finish2Vertical = {"║  ║", "║  ║", "║▒▒║", "║  ║"};
            Graphics.Add((2, SectionTypes.Finish), finish2Vertical);

            // Finish horizontal 3
            // ++++
            //  #
            //  #
            // ++++
            string[] finish3Horizontal = {"════", " ▒  ", " ▒  ", "════"};
            Graphics.Add((3, SectionTypes.Finish), finish3Horizontal);
            
            // Direction = 0
            // Starting grid vertical
            // +_ +
            // + _+
            // +_ +
            // + _+
            string[] start0Vertical = {"║_ ║", "║ _║", "║_ ║", "║ _║"};
            Graphics.Add((0, SectionTypes.StartGrid), start0Vertical);
            
            // Direction = 1
            // Starting grid horizontal
            // ++++
            //  | |
            // | |
            // ++++
            string[] start1Horizontal = {"════", " ] ]", "] ] ", "════"};
            Graphics.Add((1, SectionTypes.StartGrid), start1Horizontal);
            
            // Direction = 2
            // Starting grid vertical
            // + _+
            // +_ +
            // + _+
            // +_ +
            string[] start2Vertical = {"║ _║", "║_ ║", "║ _║", "║_ ║"};
            Graphics.Add((2, SectionTypes.StartGrid), start2Vertical);
            
            // Direction = 3
            // Starting grid horizontal
            // ++++
            // | |
            //  | |
            // ++++
            string[] start3Horizontal = {"════", "] ] ", " ] ]", "════"};
            Graphics.Add((3, SectionTypes.StartGrid), start3Horizontal);


            // Direction = 1 or 3
            // Straight horizontal
            // ++++
            //    
            //    
            // ++++
            string[] straightHorizontal = {"════", "    ", "    ", "════"};
            Graphics.Add((1, SectionTypes.Straight), straightHorizontal);
            Graphics.Add((3, SectionTypes.Straight), straightHorizontal);


            // Direction = 0 or 2
            // Straight vertical
            // +  +
            // +  +
            // +  +
            // +  +
            string[] straightVertical = {"║  ║", "║  ║", "║  ║", "║  ║"};
            Graphics.Add((0, SectionTypes.Straight), straightVertical);
            Graphics.Add((2, SectionTypes.Straight), straightVertical);

            // Direction = 0 or 2
            // Corner like a top right corner
            // ++++
            // +   
            // +   
            // +  +
            string[] cornerTopRight = {"╔═══", "║   ", "║   ", "║  ╔"};
            Graphics.Add((0, SectionTypes.RightCorner), cornerTopRight);
            Graphics.Add((3, SectionTypes.LeftCorner), cornerTopRight);

            // Direction = 0 or 1
            // Corner like a top left corner
            // ++++
            //    +
            //    +
            // +  +
            string[] cornerTopLeft = {"═══╗", "   ║", "   ║", "╗  ║"};
            Graphics.Add((0, SectionTypes.LeftCorner), cornerTopLeft);
            Graphics.Add((1, SectionTypes.RightCorner), cornerTopLeft);
            
            // Direction = 1 or 2
            // Corner like a bottom right corner
            // +  +
            //    +
            //    +
            // ++++
            string[] cornerBottomRight = {"╝  ║", "   ║", "   ║", "═══╝"};
            Graphics.Add((1, SectionTypes.LeftCorner), cornerBottomRight);
            Graphics.Add((2, SectionTypes.RightCorner), cornerBottomRight);
            
            // Direction = 2 or 3
            // Corner like a bottom left corner
            // +  +
            // +   
            // +   
            // ++++
            string[] cornerBottomLeft = {"║  ╚", "║   ", "║   ", "╚═══"};
            Graphics.Add((2, SectionTypes.LeftCorner), cornerBottomLeft);
            Graphics.Add((3, SectionTypes.RightCorner), cornerBottomLeft);
        }
        
    }
}