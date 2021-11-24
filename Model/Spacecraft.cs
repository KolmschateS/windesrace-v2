using System;
using System.Reflection.Metadata.Ecma335;

namespace Model
{
    public class Spacecraft : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
        private readonly int _minParameterValue = 20;
        private readonly int _maxParameterValue = 40;

        public Spacecraft(int quality, int performance, int speed, bool isbroken)
        {
            Quality = quality;
            Performance = performance;
            Speed = speed;
            IsBroken = isbroken;
        }

        public void RandomizeEquipment(Random random)
        {
            Quality = RandomizeParameter(Quality, random);
            Performance = RandomizeParameter(Performance, random);
            Speed = RandomizeParameter(Speed, random);
        }

        private int RandomizeParameter(int param, Random random)
        {
            // Change in paramter
            int change = random.Next(-5, 5);

            // Result of the paramter and the change
            int result = param + change;
            
            // Checks if the change + the paramater is bigger than zero or smaller than the max 
            return result > 0 && result < _maxParameterValue && result > _minParameterValue ? result : param;
        }
    }
}
