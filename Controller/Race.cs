﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;

namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Dictionary<IParticipant, int> _finishedParticipants;

        public Dictionary<IParticipant, int>? _lapCount { get; private set; }

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public static event EventHandler NextRaceEvent;

        private System.Timers.Timer _timer;

        public int Laps { get; private set; }

        public SectionData GetSectionData(Section section)
        {
            try
            {
                return _positions[section];
            }
            catch (KeyNotFoundException)
            {
                SectionData sectionData = new SectionData();
                try
                {
                    _positions.Add(section, sectionData);
                    return sectionData;
                }
                catch (System.ArgumentException)
                {
                    return _positions[section];
                }
            }
        }
        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Laps = track.Laps;
            Participants = participants;
            _finishedParticipants = new Dictionary<IParticipant, int>();

            _lapCount = new Dictionary<IParticipant, int>();
            foreach (IParticipant p in Participants)
            {
                _lapCount[p] = 0;
            }

            _random = new Random(DateTime.Now.Millisecond);
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
            _timer.AutoReset = true;
            InitializeParticipantsStartPositions();
        }
        public void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            BreakOrRepairParticipants();
            MoveParticipants();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });                     //Raise driversChanged event.
            RaceFinishedCheck();                                                                                    //Checks if Race is finished, and calls event if so.
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

            while (StartGridSections.Count > 0)
            {
                var section = StartGridSections.Pop();
                SectionData sectionData = GetSectionData(section);

                if (ParticipantsToPlace - i > 1)
                {
                    sectionData.Left = Participants[i]; i++;
                    sectionData.Right = Participants[i]; i++;
                }
                else if (ParticipantsToPlace - i == 1)
                {
                    sectionData.Left = Participants[i]; i++;
                }
            }
        }
        public Stack<Section> GetStartSections()
        {
            var sections = new Stack<Section>();
            foreach (Section S in Track.Sections)
            {
                if (S.Sectiontype == SectionTypes.StartGrid)
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
            foreach (Section S in Track.Sections)
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
            if (CurrentSectionData.Left != null)
            {
                if (!CurrentSectionData.Left.Equipement.IsBroken)
                {
                    try { CurrentSectionData.DistanceLeft += GetSpeedParticipant(CurrentSectionData.Left); }
                    catch (System.NullReferenceException) { CurrentSectionData.DistanceLeft = 0; }
                    if (CurrentSectionData.DistanceLeft >= 100)
                    {
                        if (NextSectionData.Left == null)
                        {
                            NextSectionData.Left = CurrentSectionData.Left;
                            CheckIfParticipantOnFinish(NextSection, NextSectionData.Left);
                            CurrentSectionData.Left = null;
                            CurrentSectionData.DistanceLeft = 0;
                        }
                        else if (NextSectionData.Right == null)
                        {
                            NextSectionData.Right = CurrentSectionData.Left;
                            CheckIfParticipantOnFinish(NextSection, NextSectionData.Right);
                            CurrentSectionData.Left = null;
                            CurrentSectionData.DistanceLeft = 0;
                        }
                        else
                        {
                            CurrentSectionData.DistanceLeft = 99;
                        }
                    }
                }
            }
            //RightParticipant
            if (CurrentSectionData.Right != null)
            {
                if (!CurrentSectionData.Right.Equipement.IsBroken)
                {
                    try { CurrentSectionData.DistanceRight += GetSpeedParticipant(CurrentSectionData.Right); }
                    catch (System.NullReferenceException) { CurrentSectionData.DistanceRight = 0; }
                    if (CurrentSectionData.DistanceRight >= 100)
                    {
                        if (NextSectionData.Right == null)
                        {
                            NextSectionData.Right = CurrentSectionData.Right;
                            CheckIfParticipantOnFinish(NextSection, NextSectionData.Right);
                            CurrentSectionData.Right = null;
                            CurrentSectionData.DistanceRight = 0;
                        }
                        else if (NextSectionData.Left == null)
                        {
                            NextSectionData.Left = CurrentSectionData.Right;
                            CheckIfParticipantOnFinish(NextSection, NextSectionData.Left);
                            CurrentSectionData.Right = null;
                            CurrentSectionData.DistanceRight = 0;
                        }
                        else
                        {
                            CurrentSectionData.DistanceRight = 99;
                        }
                    }
                }
            }
        }
        public void BreakOrRepairParticipants()
        {
            foreach (IParticipant participant in Participants)
            {
                BreakOrRepairSingleParticipant(participant);
            }
        }
        public void BreakOrRepairSingleParticipant(IParticipant participant)
        {
            bool BreakOrRepair = _random.Next(1, participant.Equipement.Quality) == participant.Equipement.Quality - 1 ? true : false;
            if (BreakOrRepair)
            {
                participant.Equipement.IsBroken = participant.Equipement.IsBroken ? false : true;
            }
        }
        public void CheckIfParticipantOnFinish(Section NextSection, IParticipant participant)
        {
            if (NextSection.Sectiontype == SectionTypes.Finish)
            {
                ParticipantOnFinish(NextSection, participant);
            }
        }
        public void ParticipantOnFinish(Section NextSection, IParticipant participant)
        {
            try
            {
                _lapCount[participant]++;
                if (_lapCount[participant] > Laps)
                {
                    _lapCount.Remove(participant);
                    int FinishPosition = _finishedParticipants.Count;
                    _finishedParticipants.Add(participant, FinishPosition + 1);
                    AddParticipantPoints(participant, FinishPosition);
                    RemoveParticipant(participant, GetSectionData(NextSection));
                }
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    _lapCount.Add(participant, 0);
                }
                catch (System.ArgumentException)
                {
                    _lapCount[participant]++;
                    if (_lapCount[participant] >= Laps)
                    {
                        RemoveParticipant(participant, GetSectionData(NextSection));
                    }
                }
            }
        }
        public void RemoveParticipant(IParticipant participant, SectionData sectionData)
        {
            if (sectionData.Left == participant)
            {
                sectionData.Left = null;
            }
            else if (sectionData.Right == participant)
            {
                sectionData.Right = null;
            }
        }
        public void RaceFinishedCheck()
        {
            if (isRaceFinished())
            {
                CleanUp();
                NextRaceEvent?.Invoke(this, new EventArgs());

            }
        }
        public bool isRaceFinished()
        {
            foreach (Section s in Track.Sections)
            {
                if (GetSectionData(s).Left != null || GetSectionData(s).Right != null)
                {
                    return false;
                }
            }
            return true;
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
        public void AddParticipantPoints(IParticipant Recipient, int Position)
        {
            int[] Score = {15, 10 , 8, 4, 2, 1};
            Recipient.Points += Score[Position];
        }
        public Dictionary<IParticipant, int> GetParticipantPositions()
        {
            Dictionary<IParticipant, int> positions = new Dictionary<IParticipant, int>(_finishedParticipants);
            int MaxLapCount = _lapCount.Values.Max();                                           //Max LapCount to check who is in first
            Stack<SectionData> sectionDataStack = new Stack<SectionData>();                     //Convert LinkedList into Stack
            foreach (Section s in Track.Sections)
            {
                sectionDataStack.Push(GetSectionData(s));
            }

            LinkedList<SectionData> sectionDataList = new LinkedList<SectionData>();            //Use the Stack to create a Reverse of the original LinkedList
            while (sectionDataStack.Count > 0)
            {
                sectionDataList.AddLast(sectionDataStack.Pop());
            }

            int NextPosition = (positions.Count>0) ? positions.Count + 1 : 1;
            while(positions.Count < Participants.Count)
            {
                foreach(SectionData s in sectionDataList)
                {
                    if(s.Left != null)
                    {
                        if (_lapCount[s.Left] == MaxLapCount)
                        {
                            positions.Add(s.Left, NextPosition);
                            NextPosition++;
                        }
                    }
                    if(s.Right != null)
                    {
                        if (_lapCount[s.Right] == MaxLapCount)
                        {
                            positions.Add(s.Right, NextPosition);
                            NextPosition++;
                        }
                    }
                }
                MaxLapCount--;
            }

            return positions;
        }

        public void CleanUp()
        {
            _lapCount = null;
            DriversChanged = null;
            _timer.Stop();        }
    }

}