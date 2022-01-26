using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Controller
{
    public class Race
    {
        private readonly Timer _timer;
        private const int TimerInterval = 75;

        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public event EventHandler<DriverMovedEventArgs> ADriverMoved;
        public Track Track { get; set; }
        public List<IParticipant> Pilots { get; set; }
        public List<Classification> Classifications { get; set; }
        public Dictionary<IParticipant, Classification> ClassificationsCache { get; set; }
        public DateTime StartTime { get; set; }
        private Random Random { get; set; }
        public List<IParticipant> ParticipantFinished { get; set; }
        public Dictionary<Section, SectionData> Positions { get; set; }
        private readonly int _frontStartGridDistance = 750;
        private readonly int _backStartGridDistance = 250;
        private readonly int _maxLaps = 1;
        public bool IsFinishFlagOut { get; set; }
        public int RaceLength { get; set; }

        public Race(Track track, List<IParticipant> pilots)
        {
            Random = new Random(DateTime.Now.Millisecond);
            StartTime = DateTime.Now;

            Positions = new Dictionary<Section, SectionData>();
            Track = track;
            ParticipantFinished = new List<IParticipant>();
            Classifications = new List<Classification>();
            RaceLength = Track.TrackLength * _maxLaps;

            Pilots = GenerateQualificationList(pilots);
            SetPositionsWithStartGrid(track, Pilots);

            ClassificationsCache = SetClassifications(Pilots);

            IsFinishFlagOut = false;

            _timer = new Timer(TimerInterval);
            _timer.Elapsed += OnTimedEvent;
            ADriverMoved += OnDriverMoved;
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
                    if (ParticipantFinished.Contains(pilot))
                    {
                        result.Add(pilot);
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
        private Dictionary<IParticipant, Classification> SetClassifications(List<IParticipant> participants)
        {
            Dictionary<IParticipant, Classification> classifications = new Dictionary<IParticipant, Classification>();
            foreach (IParticipant aParticipant in participants)
            {
                classifications[aParticipant] = new Classification(aParticipant, StartTime, _maxLaps);
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

        #endregion

        #region TimerStuff

        private void OnTimedEvent(object sender, ElapsedEventArgs eventArgs)
        {
            // Randomizes the equipement of the pilots
            Pilots = RandomizeEquipement(Pilots);

            // Moves the participants every timer trigger
            MoveParticipants(eventArgs.SignalTime);

            // If all participants are finished the race needs to be cleared and the next Race must be initialized
            if (AreAllParticipantsFinished())
            {
                _timer.Stop();
                AssignPointsForParticipants();
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
        public void OnDriverMoved(object sender, DriverMovedEventArgs eventArgs)
        {
            // Add passing
            ClassificationsCache[eventArgs.Pilot].Passings.Push(new Passing(eventArgs.Section, eventArgs.TimeStamp));
            ClassificationsCache[eventArgs.Pilot].Update();
            ClassificationsCache[eventArgs.Pilot].Position = ToListClassificationsCache().IndexOf(ClassificationsCache[eventArgs.Pilot]) + 1;

            // List all the classifications, so they can be read
            Classifications = ToListClassificationsCache();
        }

        private List<Classification> ToListClassificationsCache()
        {
            // First order by lapcount,
            // then order by amount of sections travelled that lap
            // then order by who finished the last section first, to determine who is in front the last section.
            return ClassificationsCache.Values.OrderByDescending(x => x.LapCount).ThenByDescending(x => x.SectionCountThisLap).ThenBy(x => x.LatestSectionTimeStamp).ToList();
        }
        public void AssignPointsForParticipants()
        {
            List<Classification> finalClassification = ClassificationsCache.Values.OrderByDescending(x => x.LapCount).ThenBy(x => x.TotalRaceTime).ToList();
            foreach (IParticipant participant in Pilots)
            {
                int Position = ClassificationsCache[participant].Position;
                int Add = AssignPoints(Position);
                participant.Points += Add;
            }
        }
        public static int AssignPoints(int position)
        {
            try
            {
                return Competition.PointSystem[position - 1];
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region ParticipantsMovement

        public void MoveParticipants(DateTime elapsedTime)
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
                    MoveParticipant(node.Value, currentSectionData, targetSection, targetSectionData, true, elapsedTime);

                if (currentSectionData.Right != null && !currentSectionData.Right.Equipment.IsBroken)
                    currentSectionData.DistanceRight
                        = SetSectionDistance(currentSectionData.Right, currentSectionData.DistanceRight);

                if (currentSectionData.DistanceRight >= Section.SectionLength)
                    MoveParticipant(node.Value, currentSectionData, targetSection, targetSectionData, false, elapsedTime);

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
            SectionData targetSectionData, bool isLeft, DateTime elapsedTime)
        {
            bool targetIsLeft = GetEmptyTargetMostToTheFront(targetSectionData);
            IParticipant participant = isLeft ? currentSectionData.Left : currentSectionData.Right;
            int currentDistance = isLeft ? currentSectionData.DistanceLeft : currentSectionData.DistanceRight;

            // Checks if the participant can be moved to a new section based on the targetSectionData of the next section
            if (CanBeMoved(targetSectionData))
            {
                ADriverMoved?.Invoke(this, new DriverMovedEventArgs(participant, currentSection, elapsedTime));
                if (currentSection.SectionType == SectionTypes.Finish)
                {
                    // Checks if the participant is finished
                    if (IsFinished(participant))
                    {
                        // Clears the participant out of the race
                        ClearFinishedSectionDataSpot(currentSection, isLeft);
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
        public void ClearFinishedSectionDataSpot(Section section, bool isLeft)
        {
            if (isLeft)
            {
                Positions[section].Left = null;
                Positions[section].DistanceLeft = 0;
            }
            else
            {
                Positions[section].Right = null;
                Positions[section].DistanceRight = 0;
            }
        }

        public bool IsFinished(IParticipant par)
        {
            if (IsFinishFlagOut || ClassificationsCache[par].Finished)
            {
                ParticipantFinished.Add(par);
                IsFinishFlagOut = true;
                return true;
            }
            return false;
        }

        public bool AreAllParticipantsFinished()
        {
            return ParticipantFinished.Count == Pilots.Count;
        }

        #endregion
    }
}