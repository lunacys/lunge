using System.Collections.Generic;

namespace lunge.Library.Entities.Systems
{
    public interface ISystemManager
    {
        /// <summary>
        /// Finds and returns <see cref="UpdateSystem"/> with specified type
        /// </summary>
        /// <typeparam name="T">Type of the system</typeparam>
        /// <returns><see cref="UpdateSystem"/></returns>
        T FindSystem<T>() where T : ISystem;

        /// <summary>
        /// Gets all registered game systems
        /// </summary>
        /// <returns>All registered game systems</returns>
        IList<ISystem> GetAllGameSystems();
    }
}
