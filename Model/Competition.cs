using System;
using System.Collections.Generic;
namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        // Return the next track in the Queue of the competition
        public Track NextTrack()
        {
            if(Tracks.Peek() == null)
            {
                return Tracks.Dequeue();
            }
            else
            {
                return null;
            }
        }
    }
}
