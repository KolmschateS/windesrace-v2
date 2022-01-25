using System;
using System.Collections.Generic;
namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public List<TeamColors> Teams { get; set; }
        public Queue<Track> Tracks { get; set; }
        public static readonly List<int> PointSystem = new List<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1};

        public Competition()
        {
            Participants = new List<IParticipant>();
            Teams = new List<TeamColors>();
            Tracks = new Queue<Track>();
        }

        // Return the next track in the Queue of the competition
        public Track NextTrack()
        {
            if (Tracks.Count != 0)
            {
                return Tracks.Dequeue();
            }
            throw(new Exception("Competition is finished"));
        }
        public int CalculateTeamPoints(TeamColors team)
        {
            int result = 0;
            foreach (IParticipant participant in Participants)
            {
                if (participant.TeamColor == team)
                {
                    result += participant.Points;
                }
            }
            return result;
        }
    }
}
