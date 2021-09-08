using System;
using System.Collections.Generic;
namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }

        // Return the next track in the Queue of the competition
        public Track NextTrack()
        {
            if (Tracks.Count != 0)
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
