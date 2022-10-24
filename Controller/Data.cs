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
        public static Competition competition;
        public static Race CurrentRace;

        public static void Initialize()
        {
            competition = new Competition();
            AddParticipants();
            AddTracks();
        }
        public static void AddParticipants()
        {
            competition.Participants.Add(new Driver("Hamilton", 0, new car(10, 5, 10), TeamColors.Blue));
            competition.Participants.Add(new Driver("Verstappen", 0, new car(10, 5, 10), TeamColors.Red));
            competition.Participants.Add(new Driver("Bottas", 0, new car(10, 5, 10), TeamColors.Yellow));
            competition.Participants.Add(new Driver("Magnussen", 0, new car(10, 5, 10), TeamColors.Green));
            competition.Participants.Add(new Driver("Schumacher", 0, new car(10, 5, 10), TeamColors.Grey));
        }
        public static void AddTracks()
        {
            competition.Tracks.Enqueue(new Track("Monaco", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
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
                SectionTypes.Straight,
                SectionTypes.Finish
            }));
            competition.Tracks.Enqueue(new Track("Zandvoord", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
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
                SectionTypes.Straight,
                SectionTypes.Finish
            }));
        }
        public static void NextRace()
        {
            Track NextTrack = competition.NextTrack();
            if(NextTrack != null)
            {
                CurrentRace = new Race(NextTrack, competition.Participants);
             }
        }
    }
}
