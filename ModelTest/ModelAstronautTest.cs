using Model;
using NUnit.Framework;

namespace ModelTest
{
    [TestFixture]
    public class ModelAstronautTest
    {
        private static Astronaut _astronaut = new Astronaut("test", 0, new Spacecraft(0,0,0,0,false), TeamColors.Red);
        [Test]
        [TestCase("test", "t")]
        [TestCase("hi", "h")]
        [TestCase("WOW", "W")]
        [TestCase("MWDGHAC", "M")]
        public void InitialTest(string input, string expected)
        {
            Assert.AreEqual(_astronaut.SetInitial(input), expected);
        }

    }
}
