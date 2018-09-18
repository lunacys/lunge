using System.Collections.Generic;

namespace lunge.Library.GameSystems
{
    public interface IGameSystemManager
    {
        /// <summary>
        /// Finds and returns <see cref="DrawableGameSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="DrawableGameSystem"/></returns>
        T FindSystem<T>() where T : DrawableGameSystem;

        /// <summary>
        /// Gets all registered game systems
        /// </summary>
        /// <returns>All registered game systems</returns>
        IList<DrawableGameSystem> GetAllGameSystems();
    }
}
