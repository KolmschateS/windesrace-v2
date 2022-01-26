using System;
using System.Collections.Generic;
using Controller;
using Model;

namespace windesrace_v2
{
    public static class Visualisation
    {
        /// <summary>
        /// Dictionary that contains all the graphic "puzzle" pieces used to build the track visualisation.
        /// Insert a direction and a sectiontype to retrieve a string[], which contains four rows containing character to be drawn for the
        /// corresponding section and direction.
        /// </summary>
        public static Dictionary<(int, SectionTypes), string[]> Graphics { get; set; }

        /// <summary>
        /// The current race being run, that needs to be drawn.
        /// </summary>
        private static Race _currentRace;

        /// <summary>
        /// Initialize class to set the currentRace with the given race
        /// </summary>
        /// <param name="race"></param>
        public static void Initialize(Race race)
        {
            _currentRace = race;
            SetGraphics();
        }

        /// <summary>
        /// Eventhandler which handles when a next race is set. It initializes the next race based on the NextRaceArgs.Race.
        /// It clears the console.
        /// It links the DriversChanged event of the currentrace to the Visualisation EventHandler OnDriversChanged
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void OnNextRace(object o, NextRaceArgs e)
        {
            Initialize(e.Race);
            Console.Clear();
            _currentRace.DriversChanged += OnDriversChanged;
        }

        /// <summary>
        /// Eventhandler which is called when the drivers have changed. It calls the DrawTrack function so the current track and participants are drawn in the console.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="eventArgs"></param>
        public static void OnDriversChanged(object o, DriversChangedEventArgs eventArgs)
        {
            DrawTrack(eventArgs.Track);
        }

        /// <summary>
        /// Function which draws each section on the console given with the Track parameter
        /// </summary>
        /// <param name="track"></param>
        public static void DrawTrack(Track track)
        {
            foreach (Section section in track.Sections)
            {
                SectionData sectionData =  _currentRace.GetSectionData(section);
                Console.SetCursorPosition(section.X, section.Y + _currentRace.Pilots.Count + 4);
                DrawSection(section, section.Direction, sectionData.Left, sectionData.Right);
            }
        }

        /// <summary>
        /// Function that draws a section on the console
        /// </summary>
        /// <param name="section"></param>
        /// <param name="direction"></param>
        /// <param name="leftParticipant"></param>
        /// <param name="rightParticipant"></param>
        public static void DrawSection(Section section, int direction, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            string[] drawString = Graphics[(direction, section.SectionType)];
            foreach (string line in drawString)
            {
                if (HasLR(line))
                // If it contains an L or R we need to check if we need to fill it with a participant
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
                // Doesn't contain a L or R, draw the line as given.
                {
                    Console.Write(line);
                }
                Console.CursorTop += 1;
                Console.CursorLeft -= 4;
            }
            Console.CursorTop -= 4;
        }

        /// <summary>
        /// Checks if a given string contains a L or R
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasLR(string input)
        {
            return input.Contains("L") || input.Contains("R");
        }

        /// <summary>
        /// Replaces an L or R in the given string. With a % is IsBroken == true. With an initial if IsBroken == false. The orginal string if an L or R is not found.
        /// </summary>
        /// <param name="sectionString"></param>
        /// <param name="IsBroken"></param>
        /// <param name="initial"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  SetGraphics is the function that sets the two-key dictionary to extract the toDraw strings[]
        /// </summary>
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