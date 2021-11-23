using NUnit.Framework;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class ModelTrackSetSectionsTest
    {
        private Track _track;
        [SetUp]
        public void Setup()
        {
            _track = new Track("TestTrack", 
                new[] {SectionTypes.Straight}, 
                0);
        }
    }
}
