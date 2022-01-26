using Model;
using NUnit.Framework;
using WPFApp;

namespace WPFAppTest
{
    public class VisualisationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        // 1 is CornerLeft
        // 2 is CornerRight
        [Test]
        [TestCase(1, 500, 45)]
        [TestCase(2, 500, 45)]
        [TestCase(1, 0, 0)]
        [TestCase(2, 0, 90)]
        [TestCase(1, 1000, 90)]
        [TestCase(2, 1000, 0)]
        public void CalculateAngleTest(int sectionType, int distance, int expectedAngle)
        {
            Assert.AreEqual(expectedAngle, Visualisation.CalculateAngle((SectionTypes)sectionType, distance));
        }

        // 1 is CornerLeft
        // 2 is CornerRight
        [Test]
        // Direction is zero
        [TestCase(0, 1, 0, 0)] // Corner is left, distance zero
        [TestCase(0, 2, 0, 0)] // Corner is right, distance zero
        [TestCase(0, 1, 500, -45)] // Corner is left, distance 500
        [TestCase(0, 2, 500, 45)] // Corner is right, distance 500
        [TestCase(0, 1, 1000, -90)] // Corner is left, distance 1000
        [TestCase(0, 2, 1000, 90)] // Corner is right, distance 1000
        [TestCase(0, 1, 750, -68)] // Corner is left, distance 750
        [TestCase(0, 2, 750, 68)] // Corner is right, distance 750

        // Direction is 1
        [TestCase(1, 1, 0, 90)] // Corner is left, distance zero
        [TestCase(1, 2, 0, 90)] // Corner is right, distance zero
        [TestCase(1, 1, 500, 45)] // Corner is left, distance 500
        [TestCase(1, 2, 500, 135)] // Corner is right, distance 500
        [TestCase(1, 1, 1000, 0)] // Corner is left, distance 1000
        [TestCase(1, 2, 1000, 180)] // Corner is right, distance 1000
        [TestCase(1, 1, 750, 22)] // Corner is left, distance 750
        [TestCase(1, 2, 750, 158)] // Corner is right, distance 750

        // Direction is 2
        [TestCase(2, 1, 0, 180)] // Corner is left, distance zero
        [TestCase(2, 2, 0, 180)] // Corner is right, distance zero
        [TestCase(2, 1, 500, 135)] // Corner is left, distance 500
        [TestCase(2, 2, 500, 225)] // Corner is right, distance 500
        [TestCase(2, 1, 1000, 90)] // Corner is left, distance 1000
        [TestCase(2, 2, 1000, 270)] // Corner is right, distance 1000
        [TestCase(2, 1, 750, 112)] // Corner is left, distance 750
        [TestCase(2, 2, 750, 248)] // Corner is right, distance 750

        // Direction is 3
        [TestCase(3, 1, 0, 270)] // Corner is left, distance zero
        [TestCase(3, 2, 0, 270)] // Corner is right, distance zero
        [TestCase(3, 1, 500, 225)] // Corner is left, distance 500
        [TestCase(3, 2, 500, 315)] // Corner is right, distance 500
        [TestCase(3, 1, 1000, 180)] // Corner is left, distance 1000
        [TestCase(3, 2, 1000, 360)] // Corner is right, distance 1000
        [TestCase(3, 1, 750, 202)] // Corner is left, distance 750
        [TestCase(3, 2, 750, 338)] // Corner is right, distance 750
        public void CalculateRotationAngleTest(int direction, int sectionType, int distance, int expectedAngle)
        {
            Assert.AreEqual(expectedAngle, Visualisation.CalculateRotationAngle(direction, (SectionTypes)sectionType, distance));
        }

        [Test]
        [TestCase(1000, 128)]
        [TestCase(500, 64)]
        [TestCase(200, 25)]
        [TestCase(254, 32)]
        [TestCase(600, 76)]
        [TestCase(0, 0)]
        public void DistanceInPixelsTest(int sectionDistance, int distanceInPixels)
        {
            Assert.AreEqual(distanceInPixels, Visualisation.DistanceInPixels(sectionDistance));
        }

        [Test]
        [TestCase(60, 1920)]
        [TestCase(0, 0)]
        [TestCase(-50, -1600)]
        [TestCase(4, 128)]
        public void CalculateCoordinateBasedOnConsoleCoordinatesTest(int inputCoord, int coordInWPF)
        {
            Assert.AreEqual(coordInWPF, Visualisation.CalculateCoordinateBasedOnConsoleCoordinates(inputCoord));
        }

        [Test]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        public void IsSectionACornerTest(int inputCorner, bool expected)
        {
            Assert.AreEqual(expected, Visualisation.IsSectionACorner((SectionTypes)inputCorner));
        }

        [Test]
        public void GetRadiusTest(int inputCorner, bool isLeft, int direction, int expected)
        {
            Assert.AreEqual(expected, Visualisation.GetRadius(isLeft));
        }
    }
}