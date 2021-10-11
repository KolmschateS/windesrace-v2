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
                Thread.Sleep(2000);
                Data.NextRace();
                try
                {
                    Visualisation.DrawTrack(Data.CurrentRace.Track);
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Races are over");
                }
            }
        }
    }
}
