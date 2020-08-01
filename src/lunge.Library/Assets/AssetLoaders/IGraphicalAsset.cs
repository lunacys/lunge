using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Assets.AssetLoaders
{
    public interface IGraphicalAsset
    {
        GraphicsDevice GraphicsDevice { get; set; }
    }
}