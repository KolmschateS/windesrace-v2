using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Model;
namespace Controller
{
    public class Race
    {
        private readonly Timer _timer;
        private const int TimerInterval = 500;

        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public Track Track { get; set; }
        public List<IParticipant> Pilots { get; set; }
        public DateTime StartTime { get; set; }
        private Random Random { get; set; } 
        private Dictionary<Section, SectionData> Positions { get; set; }
        private readonly int _frontStartGridDistance = 750;
        private readonly int _backStartGridDistance = 250;

        // Constructor
        public Race(Track track, List<IParticipant> pilots)
        {
            Random = new Random(DateTime.Now.Millisecond);
            Positions = new Dictionary<Section, SectionData>();
            StartTime = DateTime.Now;
            Track = track;
            Pilots = GenerateQualificationList(pilots);
            SetPositionsWithStartGrid(track, Pilots);

            _timer = new Timer(TimerInterval);
            _timer.Elapsed += OnTimedEvent;
            Start();
        }
        // Method to read SectionData
        public SectionData GetSectionData(Section section)
        {
            if(Positions.ContainsKey(section))
            {
                return Positions[section];
            }
            SectionData newData = new SectionData();
            Positions.Add(section, newData);
            return newData;
        }
        // Methode to randomize the equipement from opponents
        // For each pilot the equipement will be inserted with a random Integer
        public List<IParticipant> RandomizeEquipement(List<IParticipant> pilots)
        {
            if (pilots == null) return new List<IParticipant>();
            {
                List<IParticipant> result = new List<IParticipant>();
                foreach (IParticipant pilot in pilots)
                {
                    pilot.Equipment.RandomizeEquipment(Random);
                    result.Add(pilot);
                }
                return result;
            }
        }
        public List<IParticipant> GenerateQualificationList(List<IParticipant> pilots)
        {
            List<IParticipant> randomPilots = RandomizeEquipement(pilots);
            try
            { 
                return randomPilots.OrderByDescending(pilot => pilot.Equipment.Speed).Take(Track.GridSize).ToList();
            }
            catch (Exception e)
            { 
                Console.WriteLine("Whoops");
                throw;
            }
        }
        public void SetPositionsWithStartGrid(Track track, List<IParticipant> participants)
        {
            List<Section> startgrid = track.GetStartgrid();
            int place = 0;
            
            
            foreach (Section section in startgrid)
            {
                SectionData sectionData = GetSectionData(section);
                // Checks if the to allocate place is also in the participants list 
                if (place > participants.Count - 1) break; 
                sectionData.Left = participants[place]; // If so set the sectiondata with the correct participant
                sectionData.DistanceLeft = _frontStartGridDistance;
                
                // Checks if the to allocate place is also in the participants list 
                if (place + 1 > participants.Count - 1) break;
                sectionData.Right = participants[place + 1]; // If so set the sectiondata with the correct participant
                sectionData.DistanceRight = _backStartGridDistance;
                
                // Ups the place with 2 to continue to the next startgrid slots
                place += 2;
                
                // Sets the section in Positions with the filled sectionData
                Positions[section] = sectionData;
            }
        }

        #region TimerStuff
        private void OnTimedEvent(object sender, ElapsedEventArgs eventArgs)
        {
            // Moves the participants every timer trigger
            MoveParticipants();
            Pilots = RandomizeEquipement(Pilots);
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
        }

        private void Start()
        {
            _timer.Start();
        }
        #endregion

        #region ParticipantsMovement
        public void MoveParticipants()
        {
            Console.Clear();
            foreach (var position in Positions)
            {
                SectionData sectionData = position.Value;
                MoveParticipant(sectionData.Left, sectionData.DistanceLeft);
                MoveParticipant(sectionData.Right, sectionData.DistanceRight);
            }
        }
        public void MoveParticipant(IParticipant participant, int currentDistance)
        {
            int movement = CalculateMovement(participant);
            int remainder;
            Console.WriteLine($"Current distance: {currentDistance}");
            Console.WriteLine($"Current movement: {movement}");
            // If new distance exceeds max length
            if (movement + currentDistance > Section.SectionLength)
            {
                // The current player has to be moved to the next section
                // to set the correct distance the remainder has the be calculated
                remainder = movement + currentDistance - Section.SectionLength;
                Console.WriteLine($"Current remainder: {remainder}");
                // Move participant to next section of the track
                // Edit that sectionData with the new participant and the remaining 

            }
            Console.WriteLine();
        }
        // Calculates the traveled distance of a participant based on its performance and speed
        public int CalculateMovement(IParticipant participant)
        {
            return participant.Equipment.Performance * participant.Equipment.Speed;
        }
        #endregion
        
        
    }
}
