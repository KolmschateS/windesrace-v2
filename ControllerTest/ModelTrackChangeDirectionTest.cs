using NUnit.Framework;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class ModelTrackChangeDirectionTest
    {
        private Track _track;
        [SetUp]
        public void Setup()
        {
            _track = new Track("TestTrack", 
                new[] {SectionTypes.Straight}, 
                0);
        }

        [Test]
        public void DirectionIsSameAfterStraight()
        {
            int direction = 0;
            Assert.AreEqual(direction, _track.ChangeDirection(SectionTypes.Straight, direction));
        }
        
        [Test]
        public void DirectionIsSameAfterFinish()
        {
            int direction = 0;
            Assert.AreEqual(direction, _track.ChangeDirection(SectionTypes.Finish, direction));
        }
        
        [Test]
        public void DirectionIsSameAfterStartGrid()
        {
            int direction = 0;
            Assert.AreEqual(direction, _track.ChangeDirection(SectionTypes.StartGrid, direction));
        }
        
        [Test]
        public void D1AfterD0RightCorner()
        {
            int direction = 0;
            Assert.AreEqual(1, _track.ChangeDirection(SectionTypes.RightCorner, direction));
        }
        
        [Test]
        public void D1AfterD2LeftCorner()
        {
            int direction = 2;
            Assert.AreEqual(1, _track.ChangeDirection(SectionTypes.LeftCorner, direction));
        }
        
        [Test]
        public void D3AfterD0LeftCorner()
        {
            int direction = 0;
            Assert.AreEqual(3, _track.ChangeDirection(SectionTypes.LeftCorner, direction));
        }
        
        [Test]
        public void D0AfterD3RightCorner()
        {
            int direction = 3;
            Assert.AreEqual(0, _track.ChangeDirection(SectionTypes.RightCorner, direction));
        }
        
        [Test]
        public void D2AfterD3LeftCorner()
        {
            int direction = 3;
            Assert.AreEqual(2, _track.ChangeDirection(SectionTypes.LeftCorner, direction));
        }
        
        [Test]
        public void D0AfterD1LeftCorner()
        {
            int direction = 1;
            Assert.AreEqual(0, _track.ChangeDirection(SectionTypes.LeftCorner, direction));
        }
    }
}
