using System.Collections.Generic;

namespace lunge.Library.GameSystems
{
    public interface IGameSystemManager
    {
        /// <summary>
        /// Finds and returns <see cref="GameSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="GameSystem"/></returns>
        T FindSystem<T>() where T : GameSystem;

        /// <summary>
        /// Gets all registered game systems
        /// </summary>
        /// <returns>All registered game systems</returns>
        IList<GameSystem> GetAllGameSystems();
    }
}
