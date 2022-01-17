using System;
using System.Collections.Generic;
using System.Threading.Channels;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class ControllerRaceRandomizeEquipment
    {
        private List<IParticipant> _participants;
        private Race _race;
        
        [SetUp]
        public void Setup()
        {
            _participants = new List<IParticipant>();
            Astronaut astronaut = new Astronaut(
                "test", 
                50, 
                new Spacecraft(
                    50,
                    50,
                    50,
                    isBroken: false), 
                TeamColors.Blue);
            _participants.Add(astronaut);
            
            _race = new Race(new Track("test", new SectionTypes[] {SectionTypes.Straight, SectionTypes.Finish}, 0),
                _participants);
        }

        [Test]
        public void ListIsNotEquivalentAfterRandomizeEquipment()
        {
            List<IParticipant> randomizedList = _race.RandomizeEquipement(_race.Pilots);
            Assert.That(randomizedList, Is.EquivalentTo(_race.Pilots));
        }
    }
}