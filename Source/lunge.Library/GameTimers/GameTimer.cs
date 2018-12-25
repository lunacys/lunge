﻿using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameTimers
{
    public class GameTimer : ICloneable
    {
        public event EventHandler<GameTimerEventArgs> OnTimeElapsed;

        public event EventHandler<GameTimerEventArgs> OnStarted;

        public event EventHandler<GameTimerEventArgs> OnStopped;

        public double TotalSeconds { get; private set; }

        public double Interval { get; set; }

        public bool IsStopped { get; set; }

        public bool IsStarted { get; set; }

        public bool IsLooped { get; set; }

        internal bool IsExpired { get; set; }

        public GameTimer()
            : this(1.0)
        { }

        public GameTimer(double interval, bool isLooped = false)
            : this(interval, isLooped, null)
        {
            
        }

        public GameTimer(double interval, EventHandler<GameTimerEventArgs> onElapsed)
            : this(interval, true, onElapsed)
        { }

        public GameTimer(double interval, bool isLooped, EventHandler<GameTimerEventArgs> onElapsed)
        {
            Interval = interval;
            IsLooped = isLooped;
            Start();
            OnTimeElapsed = onElapsed;
        }

        public void Start()
        {
            OnStarted?.Invoke(this, new GameTimerEventArgs(TotalSeconds, Interval));
            IsStopped = false;
            IsStarted = true;
        }

        public void Stop()
        {
            OnStopped?.Invoke(this, new GameTimerEventArgs(TotalSeconds, Interval));
            IsStopped = true;
            IsStarted = false;
        }

        public void Reset()
        {
            Stop();
            TotalSeconds = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsStopped)
                TotalSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            if (!(TotalSeconds >= Interval)) return;

            OnTimeElapsed?.Invoke(this, new GameTimerEventArgs(TotalSeconds, Interval));
            TotalSeconds = 0;
            if (!IsLooped)
                Reset();
        }

        public override string ToString()
        {
            return
                $"IsStarted: {IsStarted}\nStopped: {IsStopped}\nLooped: {IsLooped}\nInterval: {Interval}\nTotal Seconds: {TotalSeconds}";
        }

        public object Clone()
        {
            var clone = new GameTimer(Interval, IsLooped)
            {
                IsStarted = IsStarted,
                IsStopped = IsStopped,
                TotalSeconds = TotalSeconds
            };

            return clone;
        }
    }
}