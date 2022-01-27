using System;
using System.Collections.Generic;
using Model;

namespace Controller
{
    /// <summary>
    /// Static class containing all the data that fills the competition
    /// </summary>
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        public static event EventHandler<NextRaceArgs> NextRaceEvent;
        public static readonly int _baseQuality = 10, _basePerformance = 10, _baseSpeed = 10, StrengthInit = 5000;
        private static Random _random { get; set; }

        /// <summary>
        /// Initializes the data class, the competition is filled
        /// </summary>
        public static void Initialize()
        {
            TrackData.Initialize();
            _random = new Random(DateTime.Now.Millisecond);
            Competition = new Competition();
            Competition.Participants = GetRandomParticipants(16);
            Competition.Teams = GetTeams(Competition.Participants);
            Competition.Tracks = EnqueueTracks(TrackData.Tracks);
        }
        /// <summary>
        /// Random particpants generator based on a given number
        /// </summary>
        /// <param name="amount">Amount of participants you want to generate</param>
        /// <returns>A list of randomly generated participants</returns>
        public static List<IParticipant> GetRandomParticipants(int amount)
        {
            List <IParticipant> participants = new List<IParticipant>();
            // Adds as much participants given with the method
            for (int i = 0; i < amount; i++)
            {
                Spacecraft spacecraft = new Spacecraft(_baseQuality, _basePerformance, _baseSpeed, StrengthInit ,isBroken: false);
                Astronaut astronaut = new Astronaut(RandomNames(10), 0, spacecraft, RandomColor());
                participants.Add(astronaut);
            }
            return participants;
        }
        /// <summary>
        /// Get a list of TeamColors currently in the participant list
        /// </summary>
        /// <param name="participants"></param>
        /// <returns>A list of which TeamColors are in the inputted list </returns>
        public static List<TeamColors> GetTeams(List<IParticipant> participants)
        {
            List<TeamColors> result = new List<TeamColors>();
            foreach(IParticipant participant in participants)
            {
                if (!result.Contains(participant.TeamColor))
                {
                    result.Add(participant.TeamColor);
                }
            }
            return result;
        }
        /// <summary>
        /// Extracts the trackdata from the TrackData class and generates a Queue
        /// </summary>
        /// <returns>A Queue with track</returns>
        public static Queue<Track> EnqueueTracks(List<Track> tracks)
        {
            Queue<Track> result = new Queue<Track>();
            foreach(Track track in tracks)
            {
                result.Enqueue(track);
            }
            return new Queue<Track>(result);
        }
        
        /// <summary>
        /// Called when a next race can be run. Cleans up the last race, sets the new race and invokes the NextRaceEvent.
        /// </summary>
        public static void SetNextRace()
        {
            // Clean up previous race
            CurrentRace?.CleanUp();
            CurrentRace = new Race(Competition.NextTrack(), ResetWearOfParticipants(Competition.Participants));
            NextRaceEvent?.Invoke(null, new NextRaceArgs(CurrentRace));
        }
        
        /// <summary>
        /// Function to generate a random name based on a given length
        /// </summary>
        /// <param name="length"></param>
        /// <returns>A random name (string)</returns>
        public static string RandomNames(int length)
        {
            const string chars = "abcdefghijklmnpqrstuvwxyz";
            const int clength = 10;

            var buffer = new char[length];
            for(var i = 0; i < length; ++i)
            {
                buffer[i] = chars[_random.Next(clength)];
            }

            return new string(buffer);
        }

        /// <summary>
        /// Resets the strength and fix of a given list of participants
        /// </summary>
        /// <param name="participants"></param>
        /// <returns>A list of particpants with the Strength en Fix values set to their initial values</returns>
        public static List<IParticipant> ResetWearOfParticipants(List<IParticipant> participants)
        {
            List<IParticipant> result = new List<IParticipant>();
            foreach (var participant in participants)
            {
                participant.Equipment.Strength = StrengthInit;
                participant.Equipment.Fix = 0;
                result.Add(participant);
            }

            return result;
        }
        /// <summary>
        /// Generates a random TeamColor
        /// </summary>
        /// <returns>A random TeamColor</returns>
        public static TeamColors RandomColor()
        {
            Array teamColors = Enum.GetValues(typeof(TeamColors));
            return (TeamColors)teamColors.GetValue(_random.Next(teamColors.Length));
        }
    }
}
