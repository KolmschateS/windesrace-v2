﻿using System;
using System.Collections.Generic;
namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public Competition()
        {
        }

        public Track NextTrack()
        {
            return null;
        }
    }
}