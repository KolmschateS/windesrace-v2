using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    /// <summary>
    /// Tests to test the Race.GenerateQualifyingFunctionUsed in the Race Constructor.
    /// An amount of participants is injected into the race. Based on the amount of startgrids in the current track the function has to return the correct amount
    /// of participants
    /// </summary>
    [TestFixture]
    public class ControllerRaceGenerateQualifying
    {
        [SetUp]
        public void Setup()
        {
            Data.Initialize();
        }

        [Test]
        public void Startgrid1Is2Count()
        {
            Track track = new Track("test", new SectionTypes[] {SectionTypes.StartGrid, SectionTypes.Finish}, 0);
            Race race = new Race(track, Data.GetRandomParticipants(12));
            Assert.AreEqual(race.Pilots.Count, 2);
        }
        
        [Test]
        public void Startgrid3Is6Count()
        {
            Track track = new Track("test", new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish
            }, 
                0);
            Race race = new Race(track, Data.GetRandomParticipants(12));
            Assert.AreEqual(race.Pilots.Count, 6);
        }
        
        [Test]
        public void Startgrid6Is12Count()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish
            }, 0);
            Race race = new Race(track, Data.GetRandomParticipants(12));
            Assert.AreEqual(race.Pilots.Count, 12);
        }
        
        [Test]
        public void Startgrid9Is12Count()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish
            }, 0);
            Race race = new Race(track, Data.GetRandomParticipants(12));
            Assert.AreEqual(race.Pilots.Count, 12);
        }
        
        [Test]
        public void Startgrid6Is4Count()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish
            }, 0);
            Race race = new Race(track, Data.GetRandomParticipants(4));
            Assert.AreEqual(race.Pilots.Count, 4);
        }
        
        [Test]
        public void NoErrorEmptyList()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish
            }, 0);
            Race race = new Race(track, Data.GetRandomParticipants(0));
            Assert.AreEqual(race.Pilots.Count, 0);
        }
        
        [Test]
        public void NoErrorNoStartgrids()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.Finish
            }, 0);
            Race race = new Race(track, Data.GetRandomParticipants(12));
            Assert.AreEqual(race.Pilots.Count, 0);
        }
        
        [Test]
        public void NoErrorNullParticipants()
        {
            Track track = new Track("test", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.Finish
            }, 0);
            Race race = new Race(track, null);
            Assert.AreEqual(race.Pilots.Count, 0);
        }
    }
}