using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        private readonly Timer _timer;
        private const int TimerInterval = 100;

        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public Track Track { get; set; }
        public List<IParticipant> Pilots { get; set; }
        public List<Classification> Classifications{ get; set; }
        public DateTime StartTime { get; set; }
        private Random Random { get; set; }
        public Dictionary<IParticipant, int> ParticipantLaps { get; set; }
        public Dictionary<IParticipant, bool> ParticipantFinished { get; set; }
        public Dictionary<IParticipant, int> ParticipantDistanceTravelled { get; set; }
        public Dictionary<Section, SectionData> Positions { get; set; }
        private readonly int _frontStartGridDistance = 750;
        private readonly int _backStartGridDistance = 250;
        private readonly int _maxLaps = 1;
        public bool IsFinishFlagOut { get; set; }
        public bool AreAllFinished { get; set; }

        // Constructor
        public Race(Track track, List<IParticipant> pilots)
        {
            Random = new Random(DateTime.Now.Millisecond);
            StartTime = DateTime.Now;

            Positions = new Dictionary<Section, SectionData>();
            Track = track;
            ParticipantFinished = new Dictionary<IParticipant, bool>();
            Pilots = GenerateQualificationList(pilots);
            SetPositionsWithStartGrid(track, Pilots);
            
            ParticipantLaps = SetParticipantLaps(Pilots);
            Classifications = SetClassifications(Pilots);

            IsFinishFlagOut = false;
            AreAllFinished = false;

            _timer = new Timer(TimerInterval);
            _timer.Elapsed += OnTimedEvent;
            Start();
        }

        // Method to read SectionData
        public SectionData GetSectionData(Section section)
        {
            if (Positions.ContainsKey(section))
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
                    if (ParticipantFinished.ContainsKey(pilot))
                    {
                        if (IsParticipantFinished(pilot))
                        {
                            result.Add(pilot);
                        }
                        else
                        {
                            pilot.Equipment.RandomizeEquipment(Random);
                            result.Add(pilot);
                        }
                    }
                    else
                    {
                        pilot.Equipment.RandomizeEquipment(Random);
                        result.Add(pilot);
                    }
                }
                return result;
            }
        }

        #region Setup

        public List<IParticipant> GenerateQualificationList(List<IParticipant> pilots)
        {
            
            List<IParticipant> randomPilots = RandomizeEquipement(pilots);
            try
            {
                return randomPilots.OrderByDescending(pilot => pilot.Equipment.Speed).Take(Track.GridSize).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                throw;
            }
        }

        private Dictionary<IParticipant, int> SetParticipantLaps(List<IParticipant> participants)
        {
            Dictionary<IParticipant, int> result = new Dictionary<IParticipant, int>();
            foreach (IParticipant aParticipant in participants)
                result[aParticipant] = 0;
            return result;
        }
        private List<Classification> SetClassifications(List<IParticipant> participants)
        {
            List<Classification> classifications = new List<Classification>();
            foreach (IParticipant aParticipant in participants)
            {
                classifications.Add(new Classification(aParticipant, 0));
            }
            return classifications;
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
                ParticipantDistanceTravelled[sectionData.Left] = CalculateParticipantDistanceAtStart(startgrid.Count, place);

                // Checks if the to allocate place is also in the participants list 
                if (place + 1 > participants.Count - 1) break;
                sectionData.Right = participants[place + 1]; // If so set the sectiondata with the correct participant
                sectionData.DistanceRight = _backStartGridDistance;
                ParticipantDistanceTravelled[sectionData.Right] = CalculateParticipantDistanceAtStart(startgrid.Count, place + 1);

                // Ups the place with 2 to continue to the next startgrid slots
                place += 2;

                // Sets the section in Positions with the filled sectionData
                Positions[section] = sectionData;
            }
        }

        #endregion

        #region TimerStuff

        private void OnTimedEvent(object sender, ElapsedEventArgs eventArgs)
        {
            // Randomizes the equipement of the pilots
            Pilots = RandomizeEquipement(Pilots);

            // Moves the participants every timer trigger
            MoveParticipants();

            // Checks if all the participants are finished
            AreAllParticipantsFinished();

            // If all participants are finished the race needs to be cleared and the next Race must be initialized
            if (AreAllFinished)
            {
                Data.SetNextRace();
            }

            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track));
        }

        private void Start()
        {
            _timer.Start();
        }

        public void CleanUp()
        {
            DriversChanged = null;
            _timer.Stop();
        }


        #endregion

        #region ClassificationRegion

        /// <summary>
        /// Calculates the distance of a participant based on its startgrid place
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="place"></param>
        /// <returns>Returns the distance a participants has travelled comparted to the last place on the startgrid</returns>
        public int CalculateParticipantDistanceAtStart(int gridSize, int place)
        {
            return (gridSize - place) * Section.SectionLength;
        }

        public void ChangeParticipantDistanceTravelled()
        {

        }

        public Classification DetermineClassification(IParticipant participant)
        {
            int pos = DeterminePosition(participant);
            Classification classification = new Classification(participant, pos);
        }

        public int DeterminePosition(IParticipant participant)
        {

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

                if (currentSectionData.Left != null && !currentSectionData.Left.Equipment.IsBroken)
                    currentSectionData.DistanceLeft =
                        SetSectionDistance(currentSectionData.Left, currentSectionData.DistanceLeft);

                if (currentSectionData.DistanceLeft >= Section.SectionLength)
                    MoveParticipant(node.Value, currentSectionData, targetSection, targetSectionData, true);

                if (currentSectionData.Right != null && !currentSectionData.Right.Equipment.IsBroken)
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
            SectionData targetSectionData, bool isLeft)
        {
            bool targetIsLeft = GetEmptyTargetMostToTheFront(targetSectionData);
            IParticipant participant = isLeft ? currentSectionData.Left : currentSectionData.Right;
            int currentDistance = isLeft ? currentSectionData.DistanceLeft : currentSectionData.DistanceRight;

            // Checks if the participant can be moved to a new section based on the targetSectionData of the next section
            if (CanBeMoved(targetSectionData))
            {
                // Checks if the participant is passing the finish line, based on the current sectiontype and next sectiontype
                if (PassingFinish(currentSection.SectionType, targetSection.SectionType))
                {
                    // Adds a lap to the current participant
                    AddLap(participant);
                    // Checks if the participant is finished
                    if (IsFinished(participant))
                    {
                        // Clears the participant out of the race
                        FinishParticipant(participant, currentSection);
                        return;
                    }
                }
                // The participant has to be moved, the participant is to be cleared of the current sectiondata and
                // and added to the next sections sectionsdata
                SetCurrentAndTargetPositions(currentSection, targetSection, participant, currentDistance, isLeft, targetIsLeft);


            }
            // Participant should have moved, but could not because the next sectiondata was full. So the distance will
            // be set to the maximum value possible: sectionlength - 1
            else
            {

                SetMaxDistance(currentSection, isLeft);
            }
        }

        public void SetCurrentAndTargetPositions(Section currentSection, Section targetSection,
            IParticipant participant, int currentDistance, bool currentIsLeft, bool targetIsLeft)
        {

            if (targetIsLeft)
            {
                Positions[targetSection].DistanceLeft = currentDistance - Section.SectionLength;
                Positions[targetSection].Left = participant;
            }
            else
            {
                Positions[targetSection].DistanceRight = currentDistance - Section.SectionLength;
                Positions[targetSection].Right = participant;
            }

            if (currentIsLeft)
            {
                Positions[currentSection].DistanceLeft = 0;
                Positions[currentSection].Left = null;
            }
            else
            {
                Positions[currentSection].DistanceRight = 0;
                Positions[currentSection].Right = null;
            }
        }

        public void SetMaxDistance(Section section, bool isLeft)
        {
            if (isLeft)
                Positions[section].DistanceLeft = Section.SectionLength - 1;
            else
                Positions[section].DistanceRight = Section.SectionLength - 1;
        }

        public bool GetEmptyTargetMostToTheFront(SectionData sectionData)
        {
            return sectionData.Left == null;
        }

        public bool CanBeMoved(SectionData targetSectionData)
        {
            return targetSectionData.Left == null || targetSectionData.Right == null;
        }

        #endregion


        #region FinishStuff

        public bool PassingFinish(SectionTypes currentSectionType, SectionTypes targetSectionType)
        {
            if (currentSectionType == SectionTypes.Finish && targetSectionType != SectionTypes.Finish)
            {
                return true;
            }
            return false;
        }

        public void FinishParticipant(IParticipant par, Section section)
        {
            ParticipantFinished[par] = true;
            Positions[section].Left = null;
            Positions[section].DistanceLeft = 0;
        }

        public void AddLap(IParticipant par)
        {
            ParticipantLaps[par]++;
        }

        public bool IsFinished(IParticipant participant)
        {
            // Checks if the finish flag is currently out
            if (!IsFinishFlagOut)
            {
                // The finish flag is not out, but a participant has reached the max laps.
                if (ParticipantLaps[participant] == _maxLaps + 1)
                {
                    // Put the finishflag out and return true as the participant is finished and return true
                    IsFinishFlagOut = true;
                    return true;
                }
            }
            // The finish flag is out, so the inserted sectionData has to be finished
            else
            {
                return true;
            }
            return false;
        }
        // TODO fix throw in ParticipantFinished[participant]
        public bool IsParticipantFinished(IParticipant participant)
        {
            return ParticipantFinished[participant];
        }

        public void AreAllParticipantsFinished()
        {
            if (ParticipantFinished.Count == Pilots.Count)
            {
                AreAllFinished = true;
            }
        }

        #endregion
    }
}