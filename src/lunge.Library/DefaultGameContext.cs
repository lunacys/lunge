using lunge.Library.Screens;
using Microsoft.Xna.Framework;

namespace lunge.Library
{
    public class DefaultGameContext : IGameContext
    {
        public int FPS { get; set; }
        public int FrameCount { get; set; }
        public GameTime GameTime { get; set; }
        public GameWindow Window { get; }
        public IScreenManager ScreenManager { get; }

        public DefaultGameContext(IScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }
    }
}