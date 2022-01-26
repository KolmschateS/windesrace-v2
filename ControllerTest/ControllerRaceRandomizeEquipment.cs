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
        private Race _race;

        [Test]
        public void ListIsNotEquivalentAfterRandomizeEquipment()
        {
            List<IParticipant> randomizedList = _race.RandomizeEquipement(_race.Pilots);
            Assert.That(randomizedList, Is.EquivalentTo(_race.Pilots));
        }
    }
}