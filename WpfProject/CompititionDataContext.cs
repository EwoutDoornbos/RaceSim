using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProject
{
    public class CompititionDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<IParticipant> ParticipantPoints  => Data.CurrentRace.Participants.OrderByDescending(x => x.Points).ToList();
        public IEnumerable<Track> TracksList => Data.competition.Tracks.ToList();

        public CompititionDataContext()
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
