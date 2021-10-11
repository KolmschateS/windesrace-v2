using System.Collections.Generic;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public int StartDirection { get; set; }

        public Track(string name, SectionTypes[] sections, int startDirection)
        {
            this.Name = name;
            Sections = SetSections(sections);
            StartDirection = startDirection;

            // Adds the sections given in the constructor to the Sections
            // linkedlist property

        }
        // Funtion to set the Sections property by looping over an Array containing the sections and inserting them
        // into the Linkedlist with sections
        public LinkedList<Section> SetSections(SectionTypes[] sections)
        {
            LinkedList<Section> sectionsList = new LinkedList<Section>();
            foreach (SectionTypes section in sections)
            {
                sectionsList.AddLast(new Section(section));
            }

            return sectionsList;
        }
    }
}
