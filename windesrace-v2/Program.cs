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
                Visualisation.DrawTrack(Data.CurrentRace);
                Thread.Sleep(4000);
            }
        }
    }
}
