using System;
using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ControllerTest
{
    [TestFixture]
    public class ModelSpacecraftRandomizeEquipment
    {
        private Spacecraft _spacecraft;
        [SetUp]
        public void SetUp()
        {
            _spacecraft = new Spacecraft(Data._baseQuality, Data._basePerformance, Data._baseSpeed, isBroken: false);
        }
        
        [Test]
        public void SpacecraftRandomizeSpeedAreNotEqual()
        {
            _spacecraft.RandomizeEquipment(new Random(DateTime.Now.Millisecond));
            Assert.AreNotEqual(Data._baseSpeed, _spacecraft.Speed);
        }
        
        [Test]
        public void SpacecraftRandomizePerformanceAreNotEqual()
        {
            _spacecraft.RandomizeEquipment(new Random(DateTime.Now.Millisecond));
            Assert.AreNotEqual(Data._basePerformance, _spacecraft.Performance);
        }
        
        [Test]
        public void SpacecraftRandomizeQualityAreNotEqual()
        {
            _spacecraft.RandomizeEquipment(new Random(DateTime.Now.Millisecond));
            Assert.AreNotEqual(Data._baseQuality, _spacecraft.Quality);
        }
    }
}