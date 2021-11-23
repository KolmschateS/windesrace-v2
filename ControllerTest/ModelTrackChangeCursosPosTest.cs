using System.IO;
using NUnit.Framework;
using Model;

namespace ControllerTest
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
        public void D0()
        {
            int[] change = _track.ChangeCursorPos(0);
            int[] expectedResult = {0, -8};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D1()
        {
            int[] change = _track.ChangeCursorPos(1);
            int[] expectedResult = {4, -4};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D2()
        {
            int[] change = _track.ChangeCursorPos(2);
            int[] expectedResult = {0, 0};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D3()
        {
            int[] change = _track.ChangeCursorPos(3);
            int[] expectedResult = {-4, -4};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D10()
        {
            int[] change = _track.ChangeCursorPos(10);
            int[] expectedResult = {10, 10};
            
            Assert.AreEqual(expectedResult,change);
        }
    }
}