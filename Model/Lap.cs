using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Lap
    {
        public string LapTimeDisplay { get; set; }
        public double LapTimeInMilliseconds { get; set; }
        public int LapCount { get; set;}
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public Lap(DateTime beginTime, DateTime endTime, int lapCount)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            LapCount = lapCount;
            LapTimeInMilliseconds = CalculateLapTime(beginTime, endTime);
            LapTimeDisplay = LapTimeToString(LapTimeInMilliseconds);
        }
        private double CalculateLapTime(DateTime beginTime, DateTime endTime)
        {
            return endTime.Subtract(beginTime).TotalMilliseconds;
        }
        private string LapTimeToString(double lapTimeInMilliseconds)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(lapTimeInMilliseconds);
            return string.Format("{0:D2}:{1:D2}:{2:D3}",
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
        }
    }
}
