using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace lunge.Library.GameTimers
{
    /// <summary>
    /// <see cref="GameTimer"/> manager
    /// </summary>
    public static class GameTimerManager
    {
        private static readonly List<GameTimer> GameTimerList = new List<GameTimer>();

        /// <summary>
        /// Adds a <see cref="GameTimer"/>
        /// </summary>
        /// <param name="gameTimer"><see cref="GameTimer"/> to add</param>
        /// <returns>Index of the added timer</returns>
        public static int Add(GameTimer gameTimer)
        {
            GameTimerList.Add(gameTimer);
            return GameTimerList.Count - 1;
        }

        /// <summary>
        /// Gets <see cref="GameTimer"/> by index
        /// </summary>
        /// <param name="index">Index of <see cref="GameTimer"/></param>
        /// <returns><see cref="GameTimer"/></returns>
        public static GameTimer Get(int index)
        {
            return GameTimerList[index];
        }

        /// <summary>
        /// Stops all timers
        /// </summary>
        public static void StopAll()
        {
            foreach (var gameTimer in GameTimerList)
            {
                gameTimer.Stop();
            }
        }

        /// <summary>
        /// Starts all timers
        /// </summary>
        public static void StartAll()
        {
            foreach (var gameTimer in GameTimerList)
            {
                gameTimer.Start();
            }
        }

        /// <summary>
        /// Clears the timer collection
        /// </summary>
        public static void Clear()
        {
            GameTimerList.Clear();
        }

        /// <summary>
        /// Updates the state of all the timers
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public static void Update(GameTime gameTime)
        {
            foreach (var gameTimer in GameTimerList)
            {
                gameTimer.Update(gameTime);
            }
        }
    }
}