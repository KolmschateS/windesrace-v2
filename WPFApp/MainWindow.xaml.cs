using Controller;
using Model;
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

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ImageCache.Initialize();
            Data.Initialize();
            Data.NextRace += OnNextRaceEvent;
            Data.SetNextRace();
        }

        private void OnNextRaceEvent(object o, NextRaceArgs e)
        {
            ImageCache.ClearCache();
            Visualisation.Initialize(e.Race);

            e.Race.DriversChanged += OnDriversChanged;
        }

        private void OnDriversChanged(object o, DriversChangedEventArgs e)
        {
            this.TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.TrackImage.Source = null;
                    this.TrackImage.Source = Visualisation.DrawTrack(e.Track);
                }));
        }
    }
}
