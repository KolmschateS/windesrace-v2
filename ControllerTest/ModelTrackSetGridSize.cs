using Model;
using NUnit.Framework;

namespace ControllerTest
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
            track.Sections.AddLast(new Section(SectionTypes.StartGrid));
            track.Sections.AddLast(new Section(SectionTypes.StartGrid));
            track.GridSize = track.SetGridSize();
            Assert.AreEqual(4, track.GridSize);
        }
    }
}