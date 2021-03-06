using Model;
using NUnit.Framework;

namespace ModelTest
{
    [TestFixture]
    public class ModelCompetitionTrackNameGet
    {
        private Track Track;
        [SetUp]
        public void SetUp()
        {
            Track = new Track("Test", new SectionTypes[] {SectionTypes.Finish}, 1);
        }

        [Test]
        public void IsNameSet()
        {
            Assert.AreEqual(Track.Name, "Test");
        }

    }
}