using System;

namespace lunge.Library.GameSystems
{
    public class GameSystemAddedEventArgs : EventArgs
    {
        public IGameSystemManager GameSystemManager { get; }
        public DrawableGameSystem DrawableGameSystem { get; }

        public GameSystemAddedEventArgs(IGameSystemManager gameSystemManager, DrawableGameSystem drawableGameSystem)
        {
            GameSystemManager = gameSystemManager;
            DrawableGameSystem = drawableGameSystem;
        }
    }
}
