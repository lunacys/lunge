using System;
using lunge.Library.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Assets.AssetLoaders
{
    [AssetLoader("Texture2D", "Images", ".png", ".jpg", ".bmp", ".gif", ".tif", ".dds")]
    public class Texture2DLoader : IAssetLoader<Texture2D>
    {
        public Texture2D LoadAsset(string assetFilePath)
        {
            return TextureStream.Load(assetFilePath);
        }
    }
}