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
            Data.NextRace += Visualisation.OnNextRace;
            Data.SetNextRace();

            for (;;)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
