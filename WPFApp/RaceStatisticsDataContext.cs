using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace WPFApp
{
    public class RaceStatisticsDataContext : INotifyPropertyChanged
    {
        // Implements the interface in an event
        public event PropertyChangedEventHandler PropertyChanged;

        public Race CurrentRace { get; set; }
        public List<IParticipant> Participants { get; set; }
        public List<Classification> Classifications { get; set; }
        public string currentTime { get; set; }
        public BitmapSource FinishImage { get; set; }
        public readonly string FinishImagePath = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\FinishFlag.png";
        public readonly string EmptyImage = @"C:\Users\skolm\source\repos\windesrace-v2\WPFApp\Graphics\EmptyImage.png";

        public void OnNextRace(object sender, NextRaceArgs e)
        {
            CurrentRace = e.Race;
            Participants = new List<IParticipant>();
            Classifications = new List<Classification>();
            e.Race.DriversChanged += OnDriversChanged;
        }

        public void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            Participants = CurrentRace.Pilots;
            Classifications = CurrentRace.Classifications;
            currentTime = CalculateCurrentRaceTime(DateTime.Now, CurrentRace.StartTime);

            // Notify values have changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        /// <summary>
        /// Calculates the passed between two given DateTimes
        /// </summary>
        /// <param name="now"></param>
        /// <param name="startTime"></param>
        /// <returns>A string in hh\:mm\:ss of the passed time between the two DateTimes</returns>
        private string CalculateCurrentRaceTime(DateTime now, DateTime startTime)
        {
            return now.Subtract(startTime).ToString(@"hh\:mm\:ss");
        }

    }

}
