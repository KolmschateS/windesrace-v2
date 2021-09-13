using Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_Participants
    {
        private Competition Competition;
        [SetUp]
        public void SetUp()
        {
            Competition = new Competition();
        }
        
        [Test]
        public void GettingParticipantsPossible()
        {
            Spacecraft spacecraft = new Spacecraft(50, 50, 50, false);
            Astronaut astronaut = new Astronaut("TestAstronaut", 0, spacecraft, TeamColors.Red);
            Competition.Participants.Add(astronaut);
            IParticipant result = Competition.Participants[0];
            Assert.AreEqual(astronaut,result);
        }
    }
}