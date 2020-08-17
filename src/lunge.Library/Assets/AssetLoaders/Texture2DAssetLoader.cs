using lunge.Library.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Assets.AssetLoaders
{
    [Asset("Texture2D", ".png", ".jpg", ".bmp", ".gif", ".tif", ".dds")]
    public class Texture2DAssetLoader : IAssetLoader<Texture2D>
    {
        public string AssetType => "Image";

        public IGame Game { get; }

        public Texture2DAssetLoader(IGame game)
        {
            Game = game;
        }
        
        public Texture2D Load(string assetName)
        {
            return TextureStream.Load(assetName);
        }

        object IAssetLoader.Load(string assetName)
        {
            return Load(assetName);
        }
    }
}