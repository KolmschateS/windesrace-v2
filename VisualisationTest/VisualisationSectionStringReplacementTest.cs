using Controller;
using NUnit.Framework;
using windesrace_v2;
using Model;

namespace ConsoleVisualisationTest
{
    [TestFixture]
    public class VisualisationSectionStringReplacementTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("----", false, " ",  "----")]
        [TestCase(" L  ", false, "T", " T  ")]
        [TestCase("  R ", false, "T", "  T ")]
        [TestCase("  R ", true, "T", "  % ")]
        [TestCase("----", true, "T", "----")]
        public void StringReplacement(string stringInput, bool IsBroken, string initials, string expected)
        {
            string output = Visualisation.SetSectionstring(stringInput, IsBroken, initials);
            Assert.AreEqual(expected, output);
        }

        [Test]
        [TestCase("----", false)]
        [TestCase("||||", false)]
        [TestCase(" L  ", true)]
        [TestCase(" R  ", true)]
        [TestCase("| L|", true)]
        [TestCase("----", false)]
        [TestCase("----", false)]
        public void HasLRTest(string input, bool expected)
        {
            bool result = Visualisation.HasLR(input);
            Assert.AreEqual(expected, result);
        }
    }
}
