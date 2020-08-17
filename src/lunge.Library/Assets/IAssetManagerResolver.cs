using Microsoft.Xna.Framework.Content;

namespace lunge.Library.Assets
{
    public interface IAssetManagerResolver
    {
        IAssetManager Resolve();
    }
}