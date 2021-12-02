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
        }

        public static void OnNextRace(object o, NextRaceArgs e)
        {
            Initialize(e.Race);
            _currentRace.DriversChanged += OnDriversChanged;
        }
        public static void OnDriversChanged(object o, DriversChangedEventArgs eventArgs)
        {
            // DrawLiveLaps();
            DrawTrack(eventArgs.Track);
        }

        public static void DrawLiveLaps()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(_currentRace.IsFinishFlagOut);
            Console.WriteLine($"{_currentRace.AreAllFinished} {_currentRace.ParticipantFinished.Count} ");

            foreach (var VARIABLE in _currentRace.ParticipantLaps)
            {
                Console.WriteLine($"{VARIABLE.Key.Name} {VARIABLE.Value} ");
                Console.Write($"{VARIABLE.Key.Equipment.IsBroken} | ");
                Console.Write($"S: {VARIABLE.Key.Equipment.Strength} | ");
                Console.Write($"F: {VARIABLE.Key.Equipment.Fix} | ");
                Console.Write($"Q: {VARIABLE.Key.Equipment.Quality} | ");
            }
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
                Console.Write(SetSectionstring(line, leftParticipant, rightParticipant));
                Console.CursorTop += 1;
                Console.CursorLeft -= 4;
            }
            Console.CursorTop -= 4;
        }

        public static string SetSectionstring(string sectionString, IParticipant left, IParticipant right)
        {
            if (left != null && left.Equipment.IsBroken && sectionString.Contains("L"))
            {
                return sectionString.Replace("L", "%");
            }
            if (right != null && right.Equipment.IsBroken && sectionString.Contains("R"))
            {
                return sectionString.Replace("R", "%");
            }
            // If the particpants != null and the string contains L, replace the L with the leftParticipant Initial
            if (left != null && sectionString.Contains("L")) return sectionString.Replace("L", left.Initial);
            
            // If the particpants != null and the string contains L, replace the L with the rightParticipant Initial
            if (right != null && sectionString.Contains("R")) return sectionString.Replace("R", right.Initial);

            return sectionString.Contains("L") ? sectionString.Replace("L", " ") : sectionString.Replace("R", " ");
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