using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;

namespace WpfProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RaceStatistics? raceStatistics;
        private CompititionStatistics? driverStatistics;
        public MainWindow()
        {
            Data.Initialize();
            ImagesCache.Initialize();


            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Race.NextRaceEvent += OnNextRace;

            InitializeComponent();
        }
        public void OnDriversChanged(Object sender, EventArgs e)
        {
            this.EmptyImage.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                this.EmptyImage.Source = null;
                if (Data.CurrentRace != null)
                {
                    this.EmptyImage.Source = VisualizationWPF.DrawTrack(Data.CurrentRace.Track);
                }
            }));
        }
        private void OnNextRace(object sender, EventArgs e)
        {
            Data.NextRace();

            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.DriversChanged += OnDriversChanged;
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_RaceStats_Click(object sender, RoutedEventArgs e)
        {
            raceStatistics = new RaceStatistics();
            raceStatistics.Show();
        }

        private void MenuItem_DriverStats_Click(object sender, RoutedEventArgs e)
        {
            driverStatistics = new CompititionStatistics();
            driverStatistics.Show();
        }

        private void MenuItem_Exit_Enter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFB70A0A");
                button.Foreground = brush;
            }
        }

        private void MenuItem_Exit_Leave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var color = (System.Windows.Media.Color)System.Windows.Media.Color.FromRgb(0,0,0); ; 
                button.Foreground = new SolidColorBrush(color);
            }
        }
    }
}