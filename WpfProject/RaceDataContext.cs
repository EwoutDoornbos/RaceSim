using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProject
{
    public class RaceDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NameTrack
        {
            get => Data.CurrentRace != null
                ? $"Track Name:\t\t{Data.CurrentRace.Track.Name}" : "All races have finished!";
        }
        public string TrackLength
        {
            get => Data.CurrentRace != null
                ? $"Track Length:\t\t {Data.CurrentRace.Track.Sections.Count * 100} Meter" : "";
        }
        public string LapCount
        {
            get => Data.CurrentRace != null
                ? $"Laps Count:\t\t {VisualizationWPF.GetLapCount(Data.CurrentRace)}/{Data.CurrentRace.Laps}" : "";
        }
        public string TrackInfo
        {
            get;
        }
        public Dictionary<IParticipant, int> Leaderboard
        {
            get => Data.CurrentRace.GetParticipantPositions();
        }
        public IEnumerable<IParticipant> EquipementStats => Data.CurrentRace.Participants.ToList();

        public RaceDataContext()
        {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriversChanged;
            }

        }

        public void OnDriversChanged(Object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
