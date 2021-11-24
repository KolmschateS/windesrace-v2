namespace Controller
{
    public class NextRaceArgs
    {
        public Race Race { get; set; }

        public NextRaceArgs(Race race)
        {
            Race = race;
        }
    }
}