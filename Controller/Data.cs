using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition competition;

        // Sort of constructor. Initializes the data used in the application
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
                competition.Participants.Add(new Pilot(
                    "Pilot" + i, // Name
                    0, // Points
                    new Spacecraft( // Equipement -> Spacecraft
                        50, // Performance
                        50, // Quality
                        50, // Speed
                        false), // Is broken
                    TeamColors.Red)); // Team color
            }
        }

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

        }
    }
}
