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
                Console.WriteLine("New race");
                Console.WriteLine(Data.CurrentRace.Track.Name);
            }
        }
    }
}
