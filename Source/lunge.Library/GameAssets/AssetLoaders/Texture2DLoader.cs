using System;
using System.IO;
using lunge.Library.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.GameAssets.AssetLoaders
{
    [AssetLoader("Texture2D", "Images", ".png", ".jpg", ".bmp", ".gif", ".tif", ".dds")]
    public class Texture2DLoader : IAssetLoader<Texture2D>, IGraphicalAsset
    {
        public GraphicsDevice GraphicsDevice { get; set; }

        public Texture2D LoadAsset(string assetFilePath)
        {
            if (GraphicsDevice == null)
                throw new InvalidOperationException("Please initialize GraphicsDevice first");

            TextureStream.GraphicsDevice = GraphicsDevice;

            return TextureStream.Load(assetFilePath);
        }
    }
}