using System;
using System.Collections.Generic;
using Controller;
using Model;

namespace windesrace_v2
{
    public static class Visualisation
    {
        // The Two-key dictionary works as following:
        // Dictionary<(int direction, Sectiontypes sectiontype), String[] SectionString>
        public static Dictionary<(int, SectionTypes), string[]> Graphics { get; set; }
        private static Race _currentRace;
        public static void Initialize(Race race)
        {
            _currentRace = race;
            SetGraphics();
            Console.Clear();
        }

        public static void OnNextRace(object o, NextRaceArgs e)
        {
            Initialize(e.Race);
            _currentRace.DriversChanged += OnDriversChanged;
        }
        public static void OnDriversChanged(object o, DriversChangedEventArgs eventArgs)
        {
            DrawTrack(eventArgs.Track);
        }

        public static void DrawTrack(Track track)
        {
            foreach (Section section in track.Sections)
            {
                SectionData sectionData =  _currentRace.GetSectionData(section);
                Console.SetCursorPosition(section.X, section.Y + _currentRace.Pilots.Count + 4);
                DrawSection(section, section.Direction, sectionData.Left, sectionData.Right);
            }
        }
        // TODO issue with a -4 being drawn
        public static void DrawSection(Section section, int direction, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            string[] drawString = Graphics[(direction, section.SectionType)];
            foreach (string line in drawString)
            {
                if (HasLR(line))
                {
                    if (line.Contains("L"))
                    {
                        if (leftParticipant != null)
                        {
                            Console.Write(SetSectionstring(line, leftParticipant.Equipment.IsBroken, leftParticipant.Initial));
                        }
                        else
                        {
                            Console.Write(SetSectionstring(line, false, " "));
                        }
                    }
                    if (line.Contains("R"))
                    {
                        if (rightParticipant != null)
                        {
                            Console.Write(SetSectionstring(line, rightParticipant.Equipment.IsBroken, rightParticipant.Initial));
                        }
                        else
                        {
                            Console.Write(SetSectionstring(line, false, " "));
                        }
                    }
                }
                else
                {
                    Console.Write(line);
                }
                Console.CursorTop += 1;
                Console.CursorLeft -= 4;
            }
            Console.CursorTop -= 4;
        }

        public static bool HasLR(string input)
        {
            return input.Contains("L") || input.Contains("R");
        }

        public static string SetSectionstring(string sectionString, bool IsBroken, string initial)
        {
            if (sectionString.Contains("L")){
                if (IsBroken)
                {
                    return sectionString.Replace("L", "%");
                }
                else
                {
                    return sectionString.Replace("L", initial);
                }
            }
            if (sectionString.Contains("R")){
                if (IsBroken)
                {
                    return sectionString.Replace("R", "%");
                }
                else
                {
                    return sectionString.Replace("R", initial);
                }
            }
            return sectionString;
        }

        // SetGraphics is the function that sets the two-key dictionary
        private static void SetGraphics()
        {
            Graphics = new Dictionary<(int, SectionTypes), string[]>();
            
            // Direction = 0
            // Finish vertical
            // +  +
            // +##+
            // +  +
            // +  +
            string[] finish0Vertical = { "║  ║", "║▒▒║", "║L ║", "║ R║"};
            Graphics.Add((0, SectionTypes.Finish), finish0Vertical);

            // Direction = 1
            // Finish horizontal
            // ++++
            //   #
            //   #
            // ++++
            string[] finish1Horizontal = { "════", " L▒ ", "R ▒ ", "════" };
            Graphics.Add((1, SectionTypes.Finish), finish1Horizontal);

            // Direction = 2
            // Finish vertical
            // +  +
            // +  +
            // +##+
            // +  +
            string[] finish2Vertical = {"║R ║", "║ L║", "║▒▒║", "║  ║"};
            Graphics.Add((2, SectionTypes.Finish), finish2Vertical);

            // Finish horizontal 3
            // ++++
            //  #
            //  #
            // ++++
            string[] finish3Horizontal = {"════", " ▒L ", " ▒ R", "════"};
            Graphics.Add((3, SectionTypes.Finish), finish3Horizontal);
            
            // Direction = 0
            // Starting grid vertical
            // +_ +
            // +L +
            // + _+
            // + R+
            string[] start0Vertical = {"║_ ║", "║L ║", "║ _║", "║ R║"};
            Graphics.Add((0, SectionTypes.StartGrid), start0Vertical);
            
            // Direction = 1
            // Starting grid horizontal
            // ++++
            //   L|
            // R|
            // ++++
            string[] start1Horizontal = {"════", "  L|", "R|  ", "════"};
            Graphics.Add((1, SectionTypes.StartGrid), start1Horizontal);
            
            // Direction = 2
            // Starting grid vertical
            // +R +
            // +_ +
            // + L+
            // + _+
            string[] start2Vertical = {"║R ║", "║_ ║", "║ L║", "║ _║"};
            Graphics.Add((2, SectionTypes.StartGrid), start2Vertical);
            
            // Direction = 3
            // Starting grid horizontal
            // ++++
            // | |
            //  | |
            // ++++
            string[] start3Horizontal = {"════", "  |R", "|L  ", "════"};
            Graphics.Add((3, SectionTypes.StartGrid), start3Horizontal);


            // Direction = 1 or 3
            // Straight horizontal
            // ++++
            //    
            //    
            // ++++
            //TODO split because of L and R
            string[] straightHorizontal = {"════", " L  ", "  R ", "════"};
            Graphics.Add((1, SectionTypes.Straight), straightHorizontal);
            Graphics.Add((3, SectionTypes.Straight), straightHorizontal);


            // Direction = 0 or 2
            // Straight vertical
            // +  +
            // +  +
            // +  +
            // +  +
            string[] straightVertical = {"║  ║", "║L ║", "║ R║", "║  ║"};
            Graphics.Add((0, SectionTypes.Straight), straightVertical);
            Graphics.Add((2, SectionTypes.Straight), straightVertical);

            // Direction = 0 or 2
            // Corner like a top right corner
            // ++++
            // +L  
            // + R
            // +  +
            string[] cornerTopRight = {"╔═══", "║L  ", "║ R ", "║  ╔"};
            Graphics.Add((0, SectionTypes.RightCorner), cornerTopRight);
            Graphics.Add((3, SectionTypes.LeftCorner), cornerTopRight);

            // Direction = 0 or 1
            // Corner like a top left corner
            // ++++
            //  L +
            //   R+
            // +  +
            string[] cornerTopLeft = {"═══╗", " L ║", "  R║", "╗  ║"};
            Graphics.Add((0, SectionTypes.LeftCorner), cornerTopLeft);
            Graphics.Add((1, SectionTypes.RightCorner), cornerTopLeft);
            
            // Direction = 1 or 2
            // Corner like a bottom right corner
            // +  +
            //  R +
            //   L+
            // ++++
            string[] cornerBottomRight = {"╝  ║", " R ║", "  L║", "═══╝"};
            Graphics.Add((1, SectionTypes.LeftCorner), cornerBottomRight);
            Graphics.Add((2, SectionTypes.RightCorner), cornerBottomRight);
            
            // Direction = 2 or 3
            // Corner like a bottom left corner
            // +  +
            // + L 
            // +R  
            // ++++
            string[] cornerBottomLeft = {"║  ╚", "║ L ", "║R  ", "╚═══"};
            Graphics.Add((2, SectionTypes.LeftCorner), cornerBottomLeft);
            Graphics.Add((3, SectionTypes.RightCorner), cornerBottomLeft);
        }
        
    }
}