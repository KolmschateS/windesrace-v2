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

        public Race(Track track, List<IParticipant> pilots)
        {
            Track = track;
            Pilots = pilots;


            _random = new Random(DateTime.Now.Millisecond);
        }

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

        public void RandomizeEquipement()
        {
            foreach (Pilot pilot in Pilots)
            {
                pilot.Equipment.Performance = _random.Next();
                pilot.Equipment.Quality = _random.Next();
                pilot.Equipment.Speed = _random.Next();
            }
        }
    }
}
