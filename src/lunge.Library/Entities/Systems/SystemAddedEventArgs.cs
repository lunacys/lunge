using System;

namespace lunge.Library.Entities.Systems
{
    public class SystemAddedEventArgs : EventArgs
    {
        public ISystemManager GameSystemManager { get; }
        public ISystem GameSystem { get; }

        public SystemAddedEventArgs(ISystemManager gameSystemManager, ISystem gameSystem)
        {
            GameSystemManager = gameSystemManager;
            GameSystem = gameSystem;
        }
    }
}
