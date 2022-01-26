using Controller;
using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest
{
    [TestFixture]
    public class ControllerDataTests
    {
        private List<IParticipant> _participantsColorTest { get; set; }
        [SetUp]
        public void Setup()
        {
            Data.Initialize();
            _participantsColorTest = new List<IParticipant>();
            _participantsColorTest.Add(new Astronaut(
                "green1",
                0,
                new Spacecraft(0,0,0,0,false),
                TeamColors.Green));

            _participantsColorTest.Add(new Astronaut(
                "green2",
                0,
                new Spacecraft(0, 0, 0, 0, false),
                TeamColors.Green));

            _participantsColorTest.Add(new Astronaut(
                "red1",
                0,
                new Spacecraft(0, 0, 0, 0, false),
                TeamColors.Red));

        }

        /// <summary>
        /// Test to check if Data.GetRandomParticipants generates the amount of participants given
        /// </summary>
        /// <param name="amountInput"></param>
        /// <param name="expected"></param>
        [Test]
        [TestCase(6, 6)]
        [TestCase(0, 0)]
        [TestCase(700, 700)]
        public void GetRandomParticipantsCountIsSameAsInput(int amountInput, int expected)
        {
            Assert.AreEqual(
            Data.GetRandomParticipants(amountInput).Count, expected);
        }

        /// <summary>
        /// Test that looks at the _participant list with the participants containing the teamcolor red and green. Checks if the Data.GetTeams returns the correct values.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expected"></param>
        [Test]
        [TestCase(TeamColors.Red, true)]
        [TestCase(TeamColors.Green, true)]
        [TestCase(TeamColors.Grey, false)]
        [TestCase(TeamColors.Blue, false)]
        [TestCase(TeamColors.Yellow, false)]
        public void GetTeamsFromParticipants(TeamColors input, bool expected)
        {
            List<TeamColors> result = Data.GetTeams(_participantsColorTest);
            Assert.AreEqual(result.Contains(input), expected);
        }
    }
}
