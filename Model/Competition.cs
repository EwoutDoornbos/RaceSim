using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants;
        public Queue<Track> Tracks;

 
        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }
        public Track NextTrack() 
        {
            try
            {
                return Tracks.Dequeue();
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }
    }
}
