    using System;
using System.Collections.Generic;
using Model;

namespace Controller
{ 
    // Class that includes data for all the Tracks
    public class TrackData
    {
        public Dictionary<String,Track> Tracks { get; set; }

        public TrackData()
        {
            Tracks = new Dictionary<string, Track>();
            Track Earth = new Track("Earth", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                
            }, 3);
            Tracks.Add(Earth.Name, Earth);

            Track Sun = new Track("Sun", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
            }, 2);
            Tracks.Add(Sun.Name, Sun);

            Track Pluto = new Track("Pluto", new SectionTypes[]
            {
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
            }, 2);
            Tracks.Add(Pluto.Name, Pluto);

            Track Mars = new Track("Mars", new SectionTypes[]
            {
                SectionTypes.Straight
            }, 1);
            Tracks.Add(Mars.Name, Mars);

            Track Uranus = new Track("Uranus", new SectionTypes[]
            {
                SectionTypes.Straight
            }, 0);
            Tracks.Add(Uranus.Name, Uranus);

            Track Jupiter = new Track("Jupiter", new SectionTypes[]
            {
                SectionTypes.Straight
            }, 1);
            Tracks.Add(Jupiter.Name, Jupiter);
        }
    }
}
