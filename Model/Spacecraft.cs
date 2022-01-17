using System;
using System.Transactions;

namespace Model
{
    public class Spacecraft : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public double FakeSpeed { get; set; }
        public bool IsBroken { get; set; }
        public int Strength { get; set; }
        public int Fix { get; set; }
        private readonly int _minParameterValue = 5;
        private readonly int _maxParameterValue = 15;
        private readonly int _fakeSpeedFactor = 62;
        private int InitStrength { get; set; }

        public Spacecraft(int quality, int performance, int speed, int initStrength, bool isBroken)
        {
            Quality = quality;
            Performance = performance;
            Speed = speed;
            FakeSpeed = Speed * _fakeSpeedFactor;
            InitStrength = initStrength;
            IsBroken = isBroken;
            Fix = 0;
        }

        public void RandomizeEquipment(Random random)
        {
            Quality = RandomizeParameter(Quality, random);
            Performance = RandomizeParameter(Performance, random);
            Speed = RandomizeParameter(Speed, random);

            Strength = DetermineStrength();
            IsBroken = DetermineIsBroken(Strength, random);


            FakeSpeed = IsBroken ? 0 : Math.Round(Speed * random.Next(_fakeSpeedFactor - 1, _fakeSpeedFactor + 1) + random.NextDouble(), 2);
        }

        private int RandomizeParameter(int param, Random random)
        {
            // Change in parameter
            int change = random.Next(0, 25) == 0 ? random.Next(-5, 5) : 0;

            // Result of the parameter and the change
            int result = param + change;
            
            // Checks if the change + the parameter is bigger than zero or smaller than the max 
            return result > 0 && result < _maxParameterValue && result > _minParameterValue ? result : param;
        }
        private bool DetermineIsBroken(int strength, Random random)
        {
            // If the spacecraft is not broken, determine it with random and the current strength of the spacecraft
            if (!IsBroken) { return random.Next(0, strength) == 0; }
            
            // The spacecraft is broken, determine if the current Fix is high enough to repair it
            if (DetermineFix(Fix, Quality) > 100)
            {
                // The fix is high enough to repair the spacecraft
                // rest the strength to 100 and return false to confirm it is not broken anymore
                Strength = InitStrength;
                Fix = 0;
                return false; 
            }
            else
            {
                Fix = DetermineFix(Fix, Quality);
                Strength = InitStrength;
            }
            return true;
        }
        private int DetermineStrength()
        {
            if (IsBroken)
            {
                return 0;
            }
            if (Strength - Quality > 0)
            {
                return Strength - Quality;
            }
            return Strength;
        }

        private int DetermineFix(int fix, int quality)
        {
            return fix + quality / 5;
        }
    }
}
