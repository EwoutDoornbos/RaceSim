using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;                                        

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        private System.Timers.Timer _timer;

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
            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
            _timer.AutoReset = true;
            InitializeParticipantsStartPositions();
        }
        public void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            MoveParticipants();
            // raise driversChanged event.
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
        }
        public void Start() { _timer.Start(); }
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
        public Stack<Section> GetStartSections()
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

        public void MoveParticipants()
        {
            
            Stack<Section> sectionstack = GetSectionStack();
            Section currentsection;
            Section previoussection = Track.Sections.First();                                          //because the stack is reversed the previous sectiondata is actualy the next section

            while (sectionstack.Count >= 1)
            {
                currentsection = sectionstack.Pop();

                MoveSingleSectionParticipants(currentsection, previoussection);

                previoussection = currentsection;
            }

        }
        //returns a SectionData Stack which is the reverse of the LinkedList in Track
        private Stack<Section> GetSectionStack()
        {
            Stack<Section> sectionStack = new Stack<Section>();
            foreach(Section S in Track.Sections)
            {
                    sectionStack.Push(S);
            }
            return sectionStack;
        }
        public void MoveSingleSectionParticipants(Section CurrentSection, Section NextSection)
        {
            //Left Participant
            SectionData CurrentSectionData = GetSectionData(CurrentSection);
            SectionData NextSectionData = GetSectionData(NextSection);
            try { CurrentSectionData.DistanceLeft += GetSpeedParticipant(CurrentSectionData.Left); }
            catch (System.NullReferenceException) { CurrentSectionData.DistanceLeft = 0; }
            if(CurrentSectionData.DistanceLeft >= 100)
            {
                if (NextSectionData.Left == null)
                {
                    NextSectionData.Left = CurrentSectionData.Left;
                    CurrentSectionData.Left = null;
                    CurrentSectionData.DistanceLeft = 0;
                }
                else if (NextSectionData.Right == null)
                {
                    NextSectionData.Right = CurrentSectionData.Left;
                    CurrentSectionData.Left = null;
                    CurrentSectionData.DistanceLeft = 0;
                }
                else
                {
                    CurrentSectionData.DistanceLeft = 99;
                }
            }
            //RightParticipant
            try { CurrentSectionData.DistanceRight += GetSpeedParticipant(CurrentSectionData.Right); }
            catch (System.NullReferenceException) { CurrentSectionData.DistanceRight = 0; }
            if (CurrentSectionData.DistanceRight >= 100)
            {
                if (NextSectionData.Right == null)
                {
                    NextSectionData.Right = CurrentSectionData.Right;
                    CurrentSectionData.Right = null;
                    CurrentSectionData.DistanceRight = 0;
                }
                else if (NextSectionData.Left == null)
                {
                    NextSectionData.Left = CurrentSectionData.Right;
                    CurrentSectionData.Right = null;
                    CurrentSectionData.DistanceRight = 0;
                }
                else
                {
                    CurrentSectionData.DistanceRight = 99;
                }
            }
        }
        public int GetSpeedParticipant(IParticipant? participant)
        {
            if (participant != null)
            {
                int P = participant.Equipement.Performance;
                int S = participant.Equipement.Speed;
                return P * S;
            }
            else
            {
                return 0;
            }
        }
    }
}