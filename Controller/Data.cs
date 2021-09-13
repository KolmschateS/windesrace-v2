using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition competition { get; set; }
        public static Race CurrentRace { get; set; }

        // Sort of a constructor. Initializes the data used in the application
        public static void Initialize()
        {
            competition = new Competition();
            AddParticipantsToCompetition(5);
            AddTracksToCompetition();
        }

        public static void AddParticipantsToCompetition(int max)
        {
            // Adds as much participants given with the method
            for (int i = 0; i < max; i++)
            {
                Spacecraft spacecraft = new Spacecraft(50, 50, 50, false);
                Astronaut astronaut = new Astronaut("Astronaut" + i, 0, spacecraft, TeamColors.Red);
                competition.Participants.Add(astronaut);
            }
        }
        // This method adds track to the competition
        public static void AddTracksToCompetition()
        {
            TrackData tracks = new TrackData();
            foreach(KeyValuePair<String, Track> track in tracks.Tracks)
            {
                competition.Tracks.Enqueue(track.Value);
            }
        }
        // Method to initiate next race.
        // TODO stop from crashing when the Competition.track Queue is empty
        public static void NextRace()
        {
            CurrentRace = new Race(competition.NextTrack(), competition.Participants);
        }
    }
}
