using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        public static event EventHandler<NextRaceArgs> NextRace;
        public static readonly int _baseQuality = 25, _basePerformance = 25, _baseSpeed = 25;

        // Sort of a constructor. Initializes the data used in the application
        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipantsToCompetition(12);
            AddTracksToCompetition();
        }

        public static void AddParticipantsToCompetition(int max)
        {
            
            // Adds as much participants given with the method
            for (int i = 0; i < max; i++)
            {
                Spacecraft spacecraft = new Spacecraft(_baseQuality, _basePerformance, _baseSpeed, false);
                Astronaut astronaut = new Astronaut(RandomNames(10), 0, spacecraft, TeamColors.Red);
                Competition.Participants.Add(astronaut);
            }
        }

        // This method adds track to the competition
        public static void AddTracksToCompetition()
        {
            TrackData tracks = new TrackData();
            foreach(Track track in tracks.Tracks)
            {
                Competition.Tracks.Enqueue(track);
            }
        }
        // Method to initiate next race.
        // TODO stop from crashing when the Competition.track Queue is empty
        public static void SetNextRace()
        {
            // Clean up previous race
            CurrentRace?.CleanUp();
            CurrentRace = new Race(Competition.NextTrack(), Competition.Participants);
            NextRace?.Invoke(null, new NextRaceArgs(CurrentRace));
        }
        
        // Random string generator for random names
        public static string RandomNames(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnpqrstuvwxyz";
            const int clength = 10;

            var buffer = new char[length];
            for(var i = 0; i < length; ++i)
            {
                buffer[i] = chars[random.Next(clength)];
            }

            return new string(buffer);
        }
    }
}
