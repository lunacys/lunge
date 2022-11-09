using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;

namespace Playground.Renderers;

public class TestRenderer : Renderer
{
    public RenderTexture? TestTexture;
    
    public TestRenderer(int renderOrder) : base(renderOrder)
    {
        WantsToRenderAfterPostProcessors = true;
    }

    public TestRenderer(int renderOrder, Camera camera) : base(renderOrder, camera)
    {
        WantsToRenderAfterPostProcessors = true;
    }

    public override void Render(Scene scene)
    {
        Core.GraphicsDevice.SetRenderTargets(TestTexture!.RenderTarget);
        
        BeginRender(Camera);
        
        
        
        EndRender();
    }

    public override void OnSceneBackBufferSizeChanged(int newWidth, int newHeight)
    {
        if (TestTexture == null)
        {
            TestTexture = new RenderTexture(newWidth, newHeight, SurfaceFormat.Color, DepthFormat.None);
        }
        else
        {
            TestTexture.OnSceneBackBufferSizeChanged(newWidth, newHeight);
        }
        
    }

    public override void Unload()
    {
        TestTexture?.Dispose();
        
        base.Unload();
    }
}