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
            // Should get the next track from the Competetition, no tracks are added so it should return null
            var result = Competition.NextTrack();
            Assert.IsNull(result);
        }
        
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            // Adds a track to the competition
            Track testTrack = new Track("testTrack", new SectionTypes[]{SectionTypes.Finish});
            Competition.Tracks.Enqueue(testTrack);
            
            // Gets the next track from the competition, should be the same track as added before.
            var result = Competition.NextTrack();
            Assert.AreEqual(testTrack, result);
        }
        
        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track testTrack = new Track("testTrack", new SectionTypes[]{SectionTypes.Finish});
            Competition.Tracks.Enqueue(testTrack);
            
            Track result = Competition.NextTrack();
            result = Competition.NextTrack();
            Assert.IsNull(result);
        }
        
        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track testTrack = new Track("testTrack", new SectionTypes[]{SectionTypes.Finish});
            Competition.Tracks.Enqueue(testTrack);
            
            Track testTrack1 = new Track("testTrack1", new SectionTypes[]{SectionTypes.Finish});
            Competition.Tracks.Enqueue(testTrack1);
            
            Track result = Competition.NextTrack();
            Track result1 = Competition.NextTrack();
            Assert.AreNotEqual(result, result1);
        }
    }
}
