using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WPFApp
{
    public class CompetitionStatisticsDataContext : INotifyPropertyChanged
    {
        // Implements the interface in an event
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IParticipant> Participants { get; set; }
        public List<TeamColors> Teams { get; set; }
        public List<Tuple<int, IParticipant>> DriverChampionship { get; set; }
        public List<Tuple<TeamColors, int, int>> TeamsChampionship { get; set; }
        public void OnNextRace(object sender, NextRaceArgs e)
        {
            Participants = Data.Competition.Participants.OrderByDescending(x => x.Points).ToList();
            Teams = Data.Competition.Teams;
            DriverChampionship = FillDriverChampionship(Participants);
            TeamsChampionship = FillTeamsChampionship(CalculateTeamPoints(Teams));


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
        private List<Tuple<int, IParticipant>> FillDriverChampionship(List<IParticipant> participants)
        {
            List<Tuple<int, IParticipant>> result = new List<Tuple<int, IParticipant>>();
            for(int i = 0; i < participants.Count; i++)
            {
                result.Add(new Tuple<int, IParticipant>(i + 1, participants[i]));
            }
            return result;
        }
        private List<Tuple<TeamColors, int>> CalculateTeamPoints(List<TeamColors> Teams)
        {
            List<Tuple<TeamColors, int>> teamPoints = new List<Tuple<TeamColors, int>>();
            foreach(var team in Teams)
            {
                int points = Data.Competition.CalculateTeamPoints(team);
                teamPoints.Add(new Tuple<TeamColors, int>(team, points));
            }
            return teamPoints.OrderByDescending(x => x.Item2).ToList();
        }
        private List<Tuple<TeamColors, int, int>> FillTeamsChampionship(List<Tuple<TeamColors, int>> teamPoints)
        {
            List<Tuple<TeamColors, int, int>> result = new List<Tuple<TeamColors, int, int>>();
            for(int i = 0; i < teamPoints.Count; i++)
            {
                TeamColors team = teamPoints[i].Item1;
                int points = teamPoints[i].Item2;
                int position = i + 1;
                result.Add(new Tuple<TeamColors, int, int>(team, points, position));
            }
            return result;
        }

    }
}
