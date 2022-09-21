using System;
using Controller;

namespace RaceSim // Note: actual namespace depends on the project name.
{
    class Program 
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRace();
            System.Console.WriteLine(Data.CurrentRace.Track.Name);

            for (; ; ) { Thread.Sleep(1000); }
        }
    }
}