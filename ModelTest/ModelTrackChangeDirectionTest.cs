using NUnit.Framework;
using Model;

namespace ModelTest
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
        [TestCase(SectionTypes.Straight, 0, 0)]
        [TestCase(SectionTypes.Finish, 0, 0)]
        [TestCase(SectionTypes.StartGrid, 0, 0)]
        [TestCase(SectionTypes.Straight, 2, 2)]
        [TestCase(SectionTypes.Finish, 2, 2)]
        [TestCase(SectionTypes.StartGrid, 2, 2)]
        [TestCase(SectionTypes.RightCorner, 0, 1)]
        [TestCase(SectionTypes.LeftCorner, 0, 3)]
        [TestCase(SectionTypes.RightCorner, 2, 3)]
        [TestCase(SectionTypes.LeftCorner, 2, 1)]
        [TestCase(SectionTypes.RightCorner, 3, 0)]
        [TestCase(SectionTypes.LeftCorner, 3, 2)]
        public void ChangeDirectionTests(SectionTypes sectiontype, int inputDirection, int expectedDirection)
        {
            Assert.AreEqual(expectedDirection, _track.ChangeDirection(sectiontype, inputDirection));
        }
    }
}
