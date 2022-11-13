using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfProject
{
    public class MainWinDataContext : INotifyPropertyChanged
    {
        public string NameTrack {
            get => Data.CurrentRace != null
                ? $"Track:\t{Data.CurrentRace.Track.Name}\nLaps:\t{VisualizationWPF.GetLapCount(Data.CurrentRace)}/{Data.CurrentRace.Laps}"
                : "All races\nhave finished!";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWinDataContext()
        {
            if(Data.CurrentRace != null)
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
