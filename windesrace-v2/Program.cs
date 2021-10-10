using System;
using Controller;
using System.Threading;

namespace windesrace_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            for (; ; )
            {
                Thread.Sleep(2000);
                Data.NextRace();
                Visualisation.Initialize();
                Visualisation.DrawTrack(Data.CurrentRace.Track);
            }
        }
    }
}
