using NUnit.Framework;
using windesrace_v2;
using Model;

namespace VisualisationTest
{
    [TestFixture]
    public class VisualisationChangeDirectionTest
    {
        [SetUp]
        public void Setup()
        {
            Visualisation.Initialize();
        }

        [Test]
        public void DirectionIsSameAfterStraight()
        {
            Visualisation.Direction = 0;
            Visualisation.ChangeDirection(SectionTypes.Straight);
            Assert.AreEqual(0, Visualisation.Direction);
        }
        
        [Test]
        public void DirectionIsSameAfterFinish()
        {
            Visualisation.Direction = 0;
            Visualisation.ChangeDirection(SectionTypes.Finish);
            Assert.AreEqual(0, Visualisation.Direction);
        }
        
        [Test]
        public void DirectionIsSameAfterStartGrid()
        {
            Visualisation.Direction = 0;
            Visualisation.ChangeDirection(SectionTypes.StartGrid);
            Assert.AreEqual(0, Visualisation.Direction);
        }
        
        [Test]
        public void D1AfterD0RightCorner()
        {
            Visualisation.Direction = 0;
            Visualisation.ChangeDirection(SectionTypes.RightCorner);
            Assert.AreEqual(1, Visualisation.Direction);
        }
        
        [Test]
        public void D1AfterD2LeftCorner()
        {
            Visualisation.Direction = 2;
            Visualisation.ChangeDirection(SectionTypes.LeftCorner);
            Assert.AreEqual(1, Visualisation.Direction);
        }
        
        [Test]
        public void D3AfterD0LeftCorner()
        {
            Visualisation.Direction = 0;
            Visualisation.ChangeDirection(SectionTypes.LeftCorner);
            Assert.AreEqual(3, Visualisation.Direction);
        }
        
        [Test]
        public void D0AfterD3RightCorner()
        {
            Visualisation.Direction = 3;
            Visualisation.ChangeDirection(SectionTypes.RightCorner);
            Assert.AreEqual(0, Visualisation.Direction);
        }
    }
}
