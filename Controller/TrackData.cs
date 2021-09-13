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
                SectionTypes.Straight
            });
            Tracks.Add(Earth.Name, Earth);

            Track Sun = new Track("Sun", new SectionTypes[]
            {
                SectionTypes.Straight
            });
            Tracks.Add(Sun.Name, Sun);

            Track Pluto = new Track("Pluto", new SectionTypes[]
            {
                SectionTypes.Straight
            });
            Tracks.Add(Pluto.Name, Pluto);

            Track Mars = new Track("Mars", new SectionTypes[]
            {
                SectionTypes.Straight
            });
            Tracks.Add(Mars.Name, Mars);

            Track Uranus = new Track("Uranus", new SectionTypes[]
            {
                SectionTypes.Straight
            });
            Tracks.Add(Uranus.Name, Uranus);

            Track Jupiter = new Track("Jupiter", new SectionTypes[]
            {
                SectionTypes.Straight
            });
            Tracks.Add(Jupiter.Name, Jupiter);
        }
    }
}
