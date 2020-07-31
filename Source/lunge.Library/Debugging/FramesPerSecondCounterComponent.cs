﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace lunge.Library.Debugging
{
    /// <summary>
    /// Smooth FPS counter. Uses the average value from the last <see cref="MaximumSamples"/>. 
    /// </summary>
    public class FramesPerSecondCounterComponent : DrawableGameComponent
    {
        #region Private Fields

        private const int RefreshesPerSecond = 16;  // how many times do we calculate FPS & UPS every second
        private readonly TimeSpan _refreshTime = TimeSpan.FromMilliseconds(1000.0f / RefreshesPerSecond);
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        #endregion

        #region Public Properties

        public event EventHandler OnFpsUpdate;

        /// <summary>
        /// Gets the current frames per second
        /// </summary>
        public float FramesPerSecond { get; protected set; }

        //public float AverageFramesPerSecond { get; private set; }

        /// <summary>
        /// Gets the current UPS.
        /// </summary>
        public float UpdatesPerSecond { get; protected set; }

        /// <summary>
        /// Total frame count past from the game start.
        /// </summary>
        public long TotalFrames { get; private set; }
        /// <summary>
        /// Total second count past from the game start.
        /// </summary>
        public float TotalSeconds { get; private set; }

        public float MinimalFps { get; private set; } = float.MaxValue;
        public float MaximalFps { get; private set; } = 0;


        private double _aggregatedFps = 0;

        #endregion

        public FramesPerSecondCounterComponent(Game game)
            : base(game)
        { }


        #region Update and Draw

        /// <summary>
        /// Allows performace monitor to calculate update rate.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > _refreshTime)
            {
                OnFpsUpdate?.Invoke(this, EventArgs.Empty);
                _elapsedTime -= _refreshTime;

                if (FramesPerSecond > MaximalFps)
                    MaximalFps = FramesPerSecond;
                else if (FramesPerSecond < MinimalFps)
                    MinimalFps = FramesPerSecond;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows performance monitor to calculate draw rate.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FramesPerSecond = 1.0f / deltaTime;

            _aggregatedFps += FramesPerSecond;

            if (TotalFrames > 0)
            {
                FramesPerSecond = (float) (_aggregatedFps / TotalFrames);
            }

            TotalFrames++;
            TotalSeconds += deltaTime;

            base.Draw(gameTime);
        }

        public override string ToString()
        {
            string outputStr = "";
            outputStr += $"FPS: {FramesPerSecond}";
            outputStr += $"\nUPS: { UpdatesPerSecond}";
            outputStr += $"\n{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024} MB";

            return outputStr;
        }

        #endregion
    }
}