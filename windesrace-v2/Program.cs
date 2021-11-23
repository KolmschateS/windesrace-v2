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
            Visualisation.Initialize();
            for (;;)
            {
                Data.NextRace();
                Thread.Sleep(5000);

                // Visualisation.DrawTrack(Data.CurrentRace.Track);
                // Visualisation.DrawRace(Data.CurrentRace);
            }
        }
    }
}
