﻿using lunge.Library.Screens;
using Microsoft.Xna.Framework;

namespace lunge.Library
{
    public interface IGameContext
    {
        int FPS { get; set; }
        int FrameCount { get; set; }
        GameTime GameTime { get; set; }
        IGameWindow Window { get; }
        Screen CurrentScreen { get; }
        ScreenManager ScreenManager { get; }
    }
}