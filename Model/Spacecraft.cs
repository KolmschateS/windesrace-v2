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
            int change = random.Next(1, 5);
            int result = 0;
            if (random.Next(0, 2) > 0)
            {
                result = param + change;
            }
            else
            {
                result = param - change;
            }

            return result;
        }
    }
}
