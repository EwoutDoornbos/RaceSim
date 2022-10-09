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
            InitializeParticipantsStartPositions();
        }
        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipement.Quality = _random.Next();
                participant.Equipement.Performance = _random.Next();
            }
        }
        public void InitializeParticipantsStartPositions()
        {
            _positions = new Dictionary<Section, SectionData>();
            Stack<Section> StartGridSections = GetStartSections();
            int i = 0;
            int ParticipantsToPlace = Participants.Count;

            while(StartGridSections.Count > 0)
            {
                var section = StartGridSections.Pop();
                SectionData sectionData = GetSectionData(section);

                if(ParticipantsToPlace-i > 1)
                {
                sectionData.Left = Participants[i]; i++;
                sectionData.Right = Participants[i]; i++;
                }
                else if(ParticipantsToPlace - i == 1)
                {
                    sectionData.Left = Participants[i]; i++;
                }
            }
        }
        private Stack<Section> GetStartSections()
        {
            var sections = new Stack<Section>();
            foreach(Section S in Track.Sections)
            {
                if(S.Sectiontype == SectionTypes.StartGrid)
                {
                    sections.Push(S);
                }
            }
            return sections;
        }
    }
}