using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Classification
    {
        public IParticipant Participant { get; set; }
        public int Position { get; set; }
        public Stack<int> LapTimes { get; set; }
        public int CurrentPoints { get; set; }
        public int ProjectedPoints { get; set; }

        public Classification(IParticipant participant, int pos)
        {
            Participant = participant;
            CurrentPoints = participant.Points;
            Position = pos;
            LapTimes = new Stack<int>();
        }
    }
}
