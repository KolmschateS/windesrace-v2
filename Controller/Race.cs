using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
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
        public Dictionary<Section, SectionData> _positions { get; set; }
        private readonly int _frontStartGridDistance = 750;
        private readonly int _backStartGridDistance = 250;

        // Constructor
        public Race(Track track, List<IParticipant> pilots)
        {
            Random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
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
            if(_positions.ContainsKey(section))
            {
                return _positions[section];
            }
            SectionData newData = new SectionData();
            _positions.Add(section, newData);
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
                _positions[section] = sectionData;
            }
        }

        #region TimerStuff
        private void OnTimedEvent(object sender, ElapsedEventArgs eventArgs)
        {
            // Randomizes the equipement of the pilots
            Pilots = RandomizeEquipement(Pilots);

            // Moves the participants every timer trigger
            MoveParticipants();
            
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
            LinkedListNode<Section> node = Track.Sections.Last;

            while (node != null)
            {
                SectionData currentSectionData = GetSectionData(node.Value);
                Section targetSection = node.Next != null ? node.Next.Value : Track.Sections.First?.Value;
                SectionData targetSectionData = GetSectionData(targetSection);

                if (currentSectionData.Left != null)
                    currentSectionData.DistanceLeft = SetSectionDistance(currentSectionData.Left, currentSectionData.DistanceLeft);

                if (currentSectionData.DistanceLeft >= Section.SectionLength)
                    MoveParticipant(node.Value, currentSectionData, targetSection, targetSectionData, true);

                if (currentSectionData.Right != null)
                    currentSectionData.DistanceRight = SetSectionDistance(currentSectionData.Right,
                        currentSectionData.DistanceRight);

                if (currentSectionData.DistanceRight >= Section.SectionLength)
                    MoveParticipant(node.Value, currentSectionData, targetSection, targetSectionData, false);

                node = node.Previous;
            }
        }

        private int CalculateMovement(IParticipant participant)
        {
            return participant.Equipment.Performance * participant.Equipment.Speed;
        }
        private int SetSectionDistance(IParticipant currentParticipant, int currentDistance)
        {
            return currentDistance + CalculateMovement(currentParticipant);
        }

        public void MoveParticipant(Section currentSection, SectionData currentSectionData, Section targetSection,
            SectionData targetSectionData, bool IsLeft)
        {
            bool moved = false;
            // Check if the target left place is empty
            if (targetSectionData.Left == null)
            {
                // The next left space is available. Place the participant there and empty the current place
                if (IsLeft)
                {
                    _positions[targetSection].DistanceLeft = currentSectionData.DistanceLeft - Section.SectionLength;
                    _positions[targetSection].Left = currentSectionData.Left;
                    _positions[currentSection].DistanceLeft = 0;
                    _positions[currentSection].Left = null;
                    moved = true;
                }
                else
                {
                    _positions[targetSection].DistanceLeft = currentSectionData.DistanceRight - Section.SectionLength;
                    _positions[targetSection].Left = currentSectionData.Right;
                    _positions[currentSection].DistanceRight= 0;
                    _positions[currentSection].Right = null;
                    moved = true;
                }
            }
            
            // Check if the target right place is empty
            else if (targetSectionData.Right == null)
            {
                // The next right space is available. Place the participant there and empty the current place.
                if (IsLeft)
                {
                    _positions[targetSection].DistanceRight = currentSectionData.DistanceLeft - Section.SectionLength;
                    _positions[targetSection].Right = currentSectionData.Left;
                    _positions[currentSection].DistanceLeft = 0;
                    _positions[currentSection].Left = null;
                    moved = true;
                }
                else
                {
                    _positions[targetSection].DistanceRight = currentSectionData.DistanceRight - Section.SectionLength;
                    _positions[targetSection].Right = currentSectionData.Right;
                    _positions[currentSection].DistanceRight= 0;
                    _positions[currentSection].Right = null;
                    moved = true;
                }
            }

            if (!moved)
            {
                if (IsLeft)
                {
                    _positions[currentSection].DistanceLeft = Section.SectionLength - 1;
                }
                else
                {
                    _positions[currentSection].DistanceRight = Section.SectionLength - 1;
                }
            }


        }
        #endregion
    }
}
