using Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ControllerTest
{
    [TestFixture]
    public class ModelCompetitionParticipants
    {
        private Competition _competition;
        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }
        
        [Test]
        public void GettingParticipantsPossible()
        {
            Spacecraft spacecraft = new Spacecraft(50, 50, 50, isBroken: false);
            Astronaut astronaut = new Astronaut("TestAstronaut", 0, spacecraft, TeamColors.Red);
            _competition.Participants.Add(astronaut);
            IParticipant result = _competition.Participants[0];
            Assert.AreEqual(astronaut,result);
        }
    }
}