using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace lunge.Library.Utils;

public static class RenderTarget2DExtensions
{
    public static void RenderFrom(this RenderTarget2D renderTarget, Batcher batcher, Action<Batcher> renderAction,
        Color? clearColor = null)
    {
        if (clearColor == null)
            clearColor = Color.Transparent;

        Core.GraphicsDevice.SetRenderTarget(renderTarget);
        Core.GraphicsDevice.Clear(clearColor.Value);

        batcher.Begin();

        renderAction(batcher);

        batcher.End();
    }

    public static void RenderFrom(this RenderTarget2D renderTarget, Action<Batcher> renderAction, Color? clearColor = null)
    {
        RenderFrom(renderTarget, Nez.Graphics.Instance.Batcher, renderAction, clearColor);
    }
}