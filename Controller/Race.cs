using System;
using System.Collections.Generic;
using Model;
namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Pilots { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random { get; set; } 
        private Dictionary<Section, SectionData> _positions { get; set; }

        // Constructor
        public Race(Track track, List<IParticipant> pilots)
        {
            Track = track;
            Pilots = pilots;
            _random = new Random(DateTime.Now.Millisecond);

            //TODO RandomizeEquipement?
        }

        // Method to read SectionData
        public SectionData GetSectionData(Section section)
        {
            if(_positions[section] == null)
            {
                SectionData newData = new SectionData();
                _positions.Add(section, newData);
                return newData;
            }
            else
            { 
                return _positions[section];
            }
        }

        // Methode to randomize the equipement from opponents
        // For each pilot the equipement will be inserted with a random Integer
        public void RandomizeEquipement()
        {
            foreach (Astronaut pilot in Pilots)
            {
                pilot.Equipment.Performance = _random.Next();
                pilot.Equipment.Quality = _random.Next();
                pilot.Equipment.Speed = _random.Next();
            }
        }
    }
}
