using System;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        public Competition Competition;
        [SetUp]
        public void SetUp()
        {
            Competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = Competition.NextTrack();
            Assert.IsNull(result);
        }
    }
}
