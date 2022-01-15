using Controller;
using Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace WPFApp
{
    public class RaceStatisticsDataContext : INotifyPropertyChanged
    {
        // Implements the interface in an event
        public event PropertyChangedEventHandler PropertyChanged;

        public Race CurrentRace {get;set;}
        public List<IParticipant> Participants {get;set;}

        public void OnNextRace(object sender, NextRaceArgs e)
        {
            CurrentRace = e.Race;
            e.Race.DriversChanged += OnDriversChanged;
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            Participants = CurrentRace.Pilots;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }

}
