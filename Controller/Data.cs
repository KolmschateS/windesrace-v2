using System;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition competition { get; set; }
        public static Race CurrentRace { get; set; }

        // Soft of a constructor. Initializes the data used in the application
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
        // TODO get tracks from seperate file
        public static void AddTracksToCompetition()
        {
            Track track1 = new Track("Track1", new SectionTypes[]
            {
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
            });
            Track track2 = new Track("Track2", new SectionTypes[]
            {
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
            });
            competition.Tracks.Enqueue(track1);
            competition.Tracks.Enqueue(track2);
        }
        // Method to initiate next race.
        public static void NextRace()
        {
            CurrentRace = new Race(competition.NextTrack(), competition.Participants);
        }
    }
}
