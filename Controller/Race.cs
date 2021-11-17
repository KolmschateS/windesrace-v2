using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using Model;
namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Pilots { get; set; }
        public DateTime StartTime { get; set; }

        private Random Random { get; set; } 
        private Dictionary<Section, SectionData> Positions { get; set; }

        // Constructor
        public Race(Track track, List<IParticipant> pilots)
        {
            Random = new Random(DateTime.Now.Millisecond);
            Positions = new Dictionary<Section, SectionData>();
            StartTime = DateTime.Now;
            Track = track;
            Pilots = GenerateQualificationList(pilots);
            SetPositions(track, Pilots);
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

        public void SetPositions(Track track, List<IParticipant> participants)
        {
            List<Section> startgrid = track.GetStartgrid();
            int place = 0;
            
            foreach (Section section in startgrid)
            {
                SectionData sectionData = GetSectionData(section);
                if (place > participants.Count - 1) break;
                sectionData.Left = participants[place];
                
                if (place + 1 > participants.Count - 1) break;
                sectionData.Right = participants[place + 1];
                place += 2;
                Positions[section] = sectionData;
            }
        }
    }
}
