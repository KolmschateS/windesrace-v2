using System;
using System.Collections.Generic;

namespace Model
{
    public class Classification
    {
        public DateTime StartTime { get; set; }
        public DateTime LatestSectionTimeStamp { get; set; }
        public string LastLapTime { get; set; }
        public string TotalRaceTime { get; set; }
        public IParticipant Participant { get; set; }
        public Stack<Passing> Passings { get; set; }
        public Stack<Lap> Laps { get; set; }
        public int LapCount { get; set; }
        public int SectionCountThisLap { get; set; }
        public int Position { get; set; }
        public bool Finished { get; set; }
        private int _maxLaps { get; set; }

        public Classification(IParticipant participant, DateTime startTime, int maxLaps)
        {
            Participant = participant;
            StartTime = startTime;

            Passings = new Stack<Passing>();
            Laps = new Stack<Lap>();

            LapCount = GetLapCount(Passings);
            SectionCountThisLap = 0;

            LastLapTime = "";
            TotalRaceTime = "";

            Finished = false;
            _maxLaps = maxLaps;

            LatestSectionTimeStamp = startTime;
        }

        public void Update()
        {
            if (Passings.Peek().Section.SectionType == SectionTypes.Finish)
            {
                Laps.Push(AddLap(Passings));
            }
            LastLapTime = Laps.Count > 0 ? Laps.Peek().LapTimeDisplay != null ? Laps.Peek().LapTimeDisplay : "" : "";
            LapCount = GetLapCount(Passings);
            Finished = LapCount >= _maxLaps;
            SectionCountThisLap++;
            LatestSectionTimeStamp = Passings.Peek().TimeStamp;

            TotalRaceTime = CalculateRaceTime(DateTime.Now, StartTime);
        }
        public Lap AddLap(Stack<Passing> passings)
        {
            SectionCountThisLap = 0;

            DateTime currentTimeStamp = passings.Peek().TimeStamp;
            DateTime lastLapTimeStamp = GetLastLapTimeStamp(passings);
            return new Lap(lastLapTimeStamp, currentTimeStamp, GetLapCount(passings));
        }
        public DateTime GetLastLapTimeStamp(Stack<Passing> passings)
        {
            int countFinishes = 0;
            foreach (Passing passing in passings)
            {
                if (passing.Section.SectionType == SectionTypes.Finish)
                {
                    countFinishes++;
                }
                if (countFinishes == 2)
                {
                    return passing.TimeStamp;
                }
            }
            return StartTime;
        }
        public int GetLapCount(Stack<Passing> passings)
        {
            int count = 0;
            foreach (Passing passing in passings)
            {
                if (passing.Section.SectionType == SectionTypes.Finish)
                {
                    count++;
                }
            }
            // Check if it not lap zero. First finish pass doesn't count as a lap so it has to be lap 0
            return count > 1 ? count - 1 : 0;
        }
        private string CalculateRaceTime(DateTime endTime, DateTime startTime)
        {
            return endTime.Subtract(startTime).ToString(@"mm\:ss\:fff");
        }
    }
}