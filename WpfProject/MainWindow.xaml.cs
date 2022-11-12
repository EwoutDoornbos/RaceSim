using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            Data.Initialize();
            ImagesCache.Initialize();


            Data.CurrentRace.DriversChanged += OnDriversChanged;

            InitializeComponent();
        }
        public void OnDriversChanged(Object sender, EventArgs e)
        {
            this.EmptyImage.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                this.EmptyImage.Source = null;
                this.EmptyImage.Source = VisualizationWPF.DrawTrack(Data.CurrentRace.Track);
            }));
        }
    }
}