using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsClass(typeof(Screen), "GameScreen")]
public static class TsGameScreen
{
    public static int Width
    {
        get => Screen.Width;
        set => Screen.Width = value;
    }
    public static int Height
    {
        get => Screen.Height;
        set => Screen.Height = value;
    }

    public static Vector2 Size => Screen.Size;
    public static Vector2 Center => Screen.Center;

    public static int PreferredBackBufferWidth
    {
        get => Screen.PreferredBackBufferWidth;
        set => Screen.PreferredBackBufferWidth = value;
    }
    public static int PreferredBackBufferHeight
    {
        get => Screen.PreferredBackBufferHeight;
        set => Screen.PreferredBackBufferHeight = value;
    }
    public static int MonitorWidth
    {
        get => Screen.MonitorWidth;
    }
    public static int MonitorHeight
    {
        get => Screen.MonitorHeight;
    }
    public static SurfaceFormat BackBufferFormat
    {
        get => Screen.BackBufferFormat;
    }
    public static SurfaceFormat PreferredBackBufferFormat
    {
        get => Screen.PreferredBackBufferFormat;
        set => Screen.PreferredBackBufferFormat = value;
    }
    public static bool SynchronizeWithVerticalRetrace
    {
        get => Screen.SynchronizeWithVerticalRetrace;
        set => Screen.SynchronizeWithVerticalRetrace = value;
    }
    public static DepthFormat PreferredDepthStencilFormat
    {
        get => Screen.PreferredDepthStencilFormat;
        set => Screen.PreferredDepthStencilFormat = value;
    }
    public static bool IsFullscreen
    {
        get => Screen.IsFullscreen;
        set => Screen.IsFullscreen = value;
    }
    public static DisplayOrientation SupportedOrientations
    {
        get => Screen.SupportedOrientations;
        set => Screen.SupportedOrientations = value;
    }

    public static void ApplyChanges() => Screen.ApplyChanges();

    /// <summary>
    /// sets the preferredBackBuffer then applies the changes
    /// </summary>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public static void SetSize(int width, int height) => Screen.SetSize(width, height);
}