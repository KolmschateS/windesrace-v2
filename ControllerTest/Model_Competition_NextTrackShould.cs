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
        
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track testTrack = new Track("testTrack", new SectionTypes[]{SectionTypes.Finish});
            Competition.Tracks.Enqueue(testTrack);
            var result = Competition.NextTrack();
            Assert.AreEqual(testTrack, result);
        }
    }
}
