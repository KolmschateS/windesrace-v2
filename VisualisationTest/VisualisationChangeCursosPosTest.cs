using System.IO;
using NUnit.Framework;
using windesrace_v2;

namespace VisualisationTest
{
    [TestFixture]
    public class VisualisationChangeCursosPosTest
    {
        [SetUp]
        public void Setup()
        {
            Visualisation.Initialize();
        }

        [Test]
        public void D0()
        {
            const int direction = 0;
            Visualisation.Direction = direction;
            int[] change = Visualisation.ChangeCursorPos();
            int[] expectedResult = {0, -8};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D1()
        {
            const int direction = 1;
            Visualisation.Direction = direction;
            int[] change = Visualisation.ChangeCursorPos();
            int[] expectedResult = {4, -4};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D2()
        {
            const int direction = 2;
            Visualisation.Direction = direction;
            int[] change = Visualisation.ChangeCursorPos();
            int[] expectedResult = {0, 0};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D3()
        {
            const int direction = 3;
            Visualisation.Direction = direction;
            int[] change = Visualisation.ChangeCursorPos();
            int[] expectedResult = {-4, -4};
            
            Assert.AreEqual(expectedResult,change);
        }
        
        [Test]
        public void D10()
        {
            const int direction = 10;
            Visualisation.Direction = direction;
            int[] change = Visualisation.ChangeCursorPos();
            int[] expectedResult = {10, 10};
            
            Assert.AreEqual(expectedResult,change);
        }
    }
}