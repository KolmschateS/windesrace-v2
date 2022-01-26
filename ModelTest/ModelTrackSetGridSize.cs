using Model;
using NUnit.Framework;

namespace ModelTest
{
    [TestFixture]
    public class ModelTrackSetGridSize
    {
        private Track track;
        [SetUp]
        public void Setup()
        {
            track = new Track("TestTrack", 
                new[] {SectionTypes.Straight}, 
                0);
        }

        [Test]
        public void SetGridSize0Test()
        {
            Assert.AreEqual(0, track.GridSize);
        }

        [Test]
        public void SetGridSize4Test()
        {
            SectionTypes[] sections =
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid
            };
            track.GridSize = track.SetGridSize(sections);
            Assert.AreEqual(4, track.GridSize);
        }
    }
}