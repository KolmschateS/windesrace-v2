using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DriverMovedEventArgs : EventArgs
    {
        public IParticipant Pilot { get; set; }
        public Section Section { get; set; }
        public DateTime TimeStamp { get; set; }

        public DriverMovedEventArgs(IParticipant pilot, Section section, DateTime timeStamp)
        {
            Pilot = pilot;
            Section = section;
            TimeStamp = timeStamp;
        }
    }
}
