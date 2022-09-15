using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class Competition
    {
        public List<IParticipant> Participants;
        public Queue<Track> Tracks;
        public NextTrack(Track track) 
        {
            Tracks = track;
        }
    }
}
