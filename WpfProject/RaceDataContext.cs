using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProject
{
    public class RaceDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TrackInfo
        {
            get;
        }
        public IEnumerable<string> Leaderboard
        {
            get;
        }
        public IEnumerable<string> EquipementStats
        {
            get;
        }

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
