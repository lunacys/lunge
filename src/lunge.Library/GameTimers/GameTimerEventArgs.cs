using System;

namespace lunge.Library.GameTimers
{
    public class GameTimerEventArgs : EventArgs
    {
        public double TotalSeconds { get; }
        public double Interval { get; }

        public GameTimerEventArgs(double totalSeconds, double interval)
        {
            TotalSeconds = totalSeconds;
            Interval = interval;
        }
    }
}