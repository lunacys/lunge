using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace LunarisGE.Library
{
    /// <summary>
    /// A game component that counts FPS and UPS (Updates Per Second).
    /// </summary>
    public class FpsCounterAdvanced : FpsCounter
    {
        #region Private Fields

        private const int RefreshesPerSecond = 4;  // how many times do we calculate FPS & UPS every second
        private readonly TimeSpan _refreshTime = TimeSpan.FromMilliseconds(1000.0f / RefreshesPerSecond);
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private int _frameCounter, _updateCounter;

        #endregion

        #region Public Properties

        public override float FramesPerSecond { get; protected set; }

        /// <summary>
        /// Gets the current UPS.
        /// </summary>
        public float UpdatesPerSecond { get; protected set; }

        #endregion

        public FpsCounterAdvanced(Game game)
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

            _updateCounter++;

            if (_elapsedTime > _refreshTime)
            {
                _elapsedTime -= _refreshTime;
                FramesPerSecond = _frameCounter * RefreshesPerSecond;
                UpdatesPerSecond = _updateCounter * RefreshesPerSecond;
                _frameCounter = 0;
                _updateCounter = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows performance monitor to calculate draw rate.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _frameCounter++;

            base.Draw(gameTime);
        }

        public override string ToString()
        {
            string outputStr = "";
            outputStr += $"FPS: {FramesPerSecond}";
            outputStr +=  $" | UPS: { UpdatesPerSecond}";
            outputStr += $" | {Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024} MB";

            return outputStr;
        }

        #endregion
    }
}