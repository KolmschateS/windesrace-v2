using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_TrackNameGet
    {
        private Track Track;
        [SetUp]
        public void SetUp()
        {
            Track = new Track("Test", new SectionTypes[] {SectionTypes.Finish});
        }

        [Test]
        public void Test()
        {
            Assert.AreEqual(Track.Name, "Test");
        }

    }
}