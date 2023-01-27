using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition CurrentCompetition;
        public static Race? CurrentRace;

        public static void Initialize()
        {
            CurrentCompetition = new Competition();
            AddParticipants();
            AddTracks();
            NextRace();
        }
        public static void AddParticipants()
        {
            CurrentCompetition.Participants.Add(new Driver("Hamilton", 0, new car(10, 10, 10), TeamColors.Blue));
            CurrentCompetition.Participants.Add(new Driver("Verstappen", 0, new car(100, 10, 10), TeamColors.Red));
            CurrentCompetition.Participants.Add(new Driver("Bottas", 0, new car(10, 10, 10), TeamColors.Yellow));
            CurrentCompetition.Participants.Add(new Driver("Magnussen", 0, new car(10, 10, 10), TeamColors.Green));
            CurrentCompetition.Participants.Add(new Driver("Schumacher", 0, new car(10, 10, 10), TeamColors.Grey));
        }
        public static void AddTracks()
        {
            CurrentCompetition.Tracks.Enqueue(new Track("Monaco", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner
/*
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight*/
            }, 2));
            CurrentCompetition.Tracks.Enqueue(new Track("Zandvoord", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight
            }, 1));
        }
        public static void NextRace()
        {
            Track NextTrack = CurrentCompetition.NextTrack();
            if(NextTrack != null)
            {
                CurrentRace = new Race(NextTrack, CurrentCompetition.Participants);
            }
            else
            {
                CurrentRace = null;
                Console.WriteLine("\n\n\tAll Races completed\n\n");
                Environment.Exit(0);
            }
        }
    }
}
