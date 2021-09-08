using System;
using System.Collections.Generic;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sections)
        {
            this.Name = name;
            Sections = new LinkedList<Section>();

            // Adds the sections given in the constructor to the Sections
            // linkedlist property
            foreach (var section in sections)
            {
                Sections.AddLast(new Section(section));
            }
        }
    }
}
