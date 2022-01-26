using System.IO;
using NUnit.Framework;
using Model;

namespace ModelTest
{
    [TestFixture]
    public class ModelTrackChangeCursosPosTest
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
        [TestCase(0, new int[] { 0, -8 })]
        [TestCase(1, new int[] { 4, -4 })]
        [TestCase(2, new int[] { 0, 0 })]
        [TestCase(3, new int[] { -4, -4 })]
        [TestCase(10, new int[] { 10, 10 })]
        public void ChangeCursosPosTest(int input, int[] expected)
        {
            int[] change = _track.ChangeCursorPos(input);
            
            Assert.AreEqual(expected, change);
        }
    }
}