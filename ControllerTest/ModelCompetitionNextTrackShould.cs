using System;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class ModelCompetitionNextTrackShould
    {
        public Competition Competition;
        [SetUp]
        public void SetUp()
        {
            Competition = new Competition();
        }

        [Test]
        public void NextTrackOneInQueueReturnTrack()
        {
            // Adds a track to the competition
            Track testTrack = new Track("testTrack", new SectionTypes[]{SectionTypes.Finish}, 1);
            Competition.Tracks.Enqueue(testTrack);
            
            // Gets the next track from the competition, should be the same track as added before.
            var result = Competition.NextTrack();
            Assert.AreEqual(testTrack, result);
        }

        [Test]
        public void NextTrackTwoInQueueReturnNextTrack()
        {
            Track testTrack = new Track("testTrack", new SectionTypes[]{SectionTypes.Finish}, 1);
            Competition.Tracks.Enqueue(testTrack);
            
            Track testTrack1 = new Track("testTrack1", new SectionTypes[]{SectionTypes.Finish}, 1);
            Competition.Tracks.Enqueue(testTrack1);
            
            Track result = Competition.NextTrack();
            Track result1 = Competition.NextTrack();
            Assert.AreNotEqual(result, result1);
        }
    }
}
