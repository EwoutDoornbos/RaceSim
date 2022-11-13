using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProject
{
    public class CompititionDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<string> Teams
        {
            get;
        }
        public string CompititionInfo
        {
            get;
        }

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
