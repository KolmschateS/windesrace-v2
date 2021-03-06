using System;
namespace Model
{
    public interface IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
        public int Strength { get; set; }
        public int Fix { get; set; }
        
        public void RandomizeEquipment(Random random);
    }
    
}
