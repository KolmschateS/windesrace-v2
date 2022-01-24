using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Passing
    {
        public Section Section { get; set; }
        public DateTime TimeStamp { get; set; }
        public Passing(Section section, DateTime dt)
        {
            Section = section;
            TimeStamp = dt;
        }
    }
}
