using System;
using System.Collections.Generic;
using System.Linq;
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
            Pilots = pilots;
            Random = new Random(DateTime.Now.Millisecond);
            Track = track;
            StartTime = DateTime.Now;
            GenerateQualificationList();
        }

        // Method to read SectionData
        public SectionData GetSectionData(Section section)
        {
            if(Positions[section] == null)
            {
                SectionData newData = new SectionData();
                Positions.Add(section, newData);
                return newData;
            }
            else
            { 
                return Positions[section];
            }
        }

        // Methode to randomize the equipement from opponents
        // For each pilot the equipement will be inserted with a random Integer
        public void RandomizeEquipement()
        {
            Pilots.ForEach(i => Console.Write("{0}\n", i.Name + " " + i.Equipment.Speed));
            foreach (IParticipant pilot in Pilots)
            {
                pilot.Equipment.RandomizeEquipment(Random);
            }
            Pilots.ForEach(i => Console.Write("{0}\n", i.Name + " " + i.Equipment.Speed));
        }

        public void GenerateQualificationList()
        {
            RandomizeEquipement();
            Console.WriteLine();
            Pilots.ForEach(i => Console.Write("{0}\n", i.Name + " " + i.Equipment.Speed));
            Pilots = Pilots.OrderByDescending(o => o.Equipment.Speed).Take(Track.GridSize).ToList();
            Console.WriteLine(Track.Name + "Qualification");
            Pilots.ForEach(i => Console.Write("{0}\n", i.Name + " " + i.Equipment.Speed));
        }
    }
}
