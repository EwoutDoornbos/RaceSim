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
            //system.console.writeline(data.currentrace.track.name);

            visualization.Initialize();
            visualization.DrawTrack(Data.CurrentRace.Track);

            for (; ; ) { Thread.Sleep(1000); }
        }
    }
}