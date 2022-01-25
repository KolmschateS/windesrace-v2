using System;
using Controller;
using System.Threading;

namespace windesrace_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init of the static Data class
            Data.Initialize();
            Data.NextRaceEvent += Visualisation.OnNextRace;
            Data.SetNextRace();

            // Loop to keep the application running
            for (;;)
            {
                Thread.Sleep(1500);
            }
        }
    }
}
