using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    internal static class Data
    {
        public static Competition competition;

        public static void Initialize()
        {
            competition = new Competition();
        }
        public static void AddParticipants()
        {
            IParticipant participant1 = new Driver();
            IParticipant participant2 = new Driver();
            IParticipant participant3 = new Driver();
            competition.Participants.Add(participant1);
            competition.Participants.Add(participant2);
            competition.Participants.Add(participant3);
        }
        public static void AddTracks()
        {
            Track track1 = new Track();
            Track track2 = new Track();
            competition.Tracks.Enqueue(track1);
            competition.Tracks.Enqueue(track2);
        }
    }
}
