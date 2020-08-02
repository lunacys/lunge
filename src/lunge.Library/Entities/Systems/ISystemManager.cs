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
    }
}
