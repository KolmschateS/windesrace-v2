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
            _spacecraft = new Spacecraft(50, 50, 50, false);
        }
        
        [Test]
        public void SpacecraftRandomizeSpeedAreNotEqual()
        {
            _spacecraft.RandomizeEquipment(new Random(DateTime.Now.Millisecond));
            Assert.AreNotEqual(50, _spacecraft.Speed);
        }
        
        [Test]
        public void SpacecraftRandomizePerformanceAreNotEqual()
        {
            _spacecraft.RandomizeEquipment(new Random(DateTime.Now.Millisecond));
            Assert.AreNotEqual(50, _spacecraft.Performance);
        }
        
        [Test]
        public void SpacecraftRandomizeQualityAreNotEqual()
        {
            _spacecraft.RandomizeEquipment(new Random(DateTime.Now.Millisecond));
            Assert.AreNotEqual(50, _spacecraft.Quality);
        }
    }
}