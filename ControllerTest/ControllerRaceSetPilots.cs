using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class ControllerRaceSetPilots
    {
        private Track track;
        private IParticipant _pilots;
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
          
        }
    }
}