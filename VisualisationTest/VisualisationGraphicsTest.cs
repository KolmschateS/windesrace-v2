using Controller;
using NUnit.Framework;
using windesrace_v2;
using Model;

namespace 
    ConsoleVisualisationTest
{
    // Test to test if the Graphics are loaded in correctly
    // In visualisation in Initialize the SetGraphics function is called
    // This function loads the graphics in a two dimensional dictionary
    // In this test we will call initialize and test if the dictionary returns
    // the correct values
    [TestFixture]
    public class VisualisationGraphicsTest
    {

        [SetUp]
        public void Setup()
        {
            Data.Initialize();
            Data.SetNextRace();
            Visualisation.Initialize(Data.CurrentRace);
        }

        // For all tests
        // D# Number is for the direction
        // after that comes the SectionType

        #region GraphicsTest

        [Test]
        public void D0FinishVertical()
        {
            string[] actual = Visualisation.Graphics[(0, SectionTypes.Finish)];
            string[] expected = { "║  ║", "║▒▒║", "║L ║", "║ R║" };
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void D1FinishHorizontal()
        {
            string[] actual = Visualisation.Graphics[(1, SectionTypes.Finish)];
            string[] expected = { "════", " L▒ ", "R ▒ ", "════" };

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D2FinishVertical()
        {
            string[] actual = Visualisation.Graphics[(2, SectionTypes.Finish)];
            string[] expected = {"║R ║", "║ L║", "║▒▒║", "║  ║"};

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void D3FinishHorizontal()
        {
            string[] actual = Visualisation.Graphics[(3, SectionTypes.Finish)];
            string[] expected = {"════", " ▒L ", " ▒ R", "════"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D0StartVertical()
        {
            string[] actual = Visualisation.Graphics[(0, SectionTypes.StartGrid)];
            string[] expected = {"║_ ║", "║L ║", "║ _║", "║ R║"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D1StartHorizontal()
        {
            string[] actual = Visualisation.Graphics[(1, SectionTypes.StartGrid)];
            string[] expected = {"════", "  L|", "R|  ", "════"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D2StartVertical()
        {
            string[] actual = Visualisation.Graphics[(2, SectionTypes.StartGrid)];
            string[] expected = {"║R ║", "║_ ║", "║ L║", "║ _║"};

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void D3StartHorizontal()
        {
            string[] actual = Visualisation.Graphics[(3, SectionTypes.StartGrid)];
            string[] expected = {"════", "  |R", "|L  ", "════"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D0StraightVertical()
        {
            string[] actual = Visualisation.Graphics[(0, SectionTypes.Straight)];
            string[] expected = {"║  ║", "║L ║", "║ R║", "║  ║"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D1StraightHorizontal()
        {
            string[] actual = Visualisation.Graphics[(1, SectionTypes.Straight)];
            string[] expected = {"════", " L  ", "  R ", "════"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D2StraightVertical()
        {
            string[] actual = Visualisation.Graphics[(2, SectionTypes.Straight)];
            string[] expected = {"║  ║", "║L ║", "║ R║", "║  ║"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D3StraightHorizontal()
        {
            string[] actual = Visualisation.Graphics[(3, SectionTypes.Straight)];
            string[] expected = {"════", " L  ", "  R ", "════"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D0LeftCorner()
        {
            string[] actual = Visualisation.Graphics[(0, SectionTypes.LeftCorner)];
            string[] expected = {"═══╗", " L ║", "  R║", "╗  ║"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D1LeftCorner()
        {
            string[] actual = Visualisation.Graphics[(1, SectionTypes.LeftCorner)];
            string[] expected = {"╝  ║", " R ║", "  L║", "═══╝"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D2LeftCorner()
        {
            string[] actual = Visualisation.Graphics[(2, SectionTypes.LeftCorner)];
            string[] expected = {"║  ╚", "║ L ", "║R  ", "╚═══"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D3LeftCorner()
        {
            string[] actual = Visualisation.Graphics[(3, SectionTypes.LeftCorner)];
            string[] expected = {"╔═══", "║L  ", "║ R ", "║  ╔"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D0RightCorner()
        {
            string[] actual = Visualisation.Graphics[(0, SectionTypes.RightCorner)];
            string[] expected = {"╔═══", "║L  ", "║ R ", "║  ╔"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D1RightCorner()
        {
            string[] actual = Visualisation.Graphics[(1, SectionTypes.RightCorner)];
            string[] expected = {"═══╗", " L ║", "  R║", "╗  ║"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D2RightCorner()
        {
            string[] actual = Visualisation.Graphics[(2, SectionTypes.RightCorner)];
            string[] expected = {"╝  ║", " R ║", "  L║", "═══╝"};

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void D3RightCorner()
        {
            string[] actual = Visualisation.Graphics[(3, SectionTypes.RightCorner)];
            string[] expected = {"║  ╚", "║ L ", "║R  ", "╚═══"};

            Assert.AreEqual(expected, actual);
        }


        #endregion
    }
}