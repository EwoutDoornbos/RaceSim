using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public SectionData GetSectionData(Section section)
        {
            try
            {
                return _positions[section];
            }
            catch (KeyNotFoundException)
            {
                SectionData sectionData = new SectionData();
                _positions.Add(section, sectionData);
                return sectionData;
            }
        }
        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
        }
        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipement.Quality = _random.Next();
                participant.Equipement.Performance = _random.Next();
            }
        }
    }
}