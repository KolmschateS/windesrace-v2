using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WPFApp
{
    public class CompetitionStatisticsDataContext : INotifyPropertyChanged
    {
        // Implements the interface in an event
        public event PropertyChangedEventHandler PropertyChanged;

        public Competition Competition { get; set; }

    }
}
