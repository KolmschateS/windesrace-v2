using System;
namespace Model
{
    public class Spacecraft : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public Spacecraft(int quality, int performance, int speed, bool isbroken)
        {
            this.Quality = quality;
            this.Performance = performance;
            this.Speed = speed;
            this.IsBroken = isbroken;
        }

    }
}
