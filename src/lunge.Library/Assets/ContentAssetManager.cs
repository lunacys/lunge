using System.IO;
using Microsoft.Xna.Framework.Content;

namespace lunge.Library.Assets
{
    public class ContentAssetManager : IAssetManager
    {
        public ContentManager Content { get; }

        public string AssetDirectory { get; set; }

        public ContentAssetManager(ContentManager contentManager, string contentDirectory = "Content", string assetDirectory = "")
        {
            AssetDirectory = assetDirectory;
            Content = contentManager;
            Content.RootDirectory = contentDirectory;
        }

        public T Load<T>(string assetName)
        {
            return Content.Load<T>(!string.IsNullOrEmpty(AssetDirectory) ? Path.Combine(AssetDirectory, assetName) : assetName);
        }

        public void Dispose()
        {
            Content.Dispose();
        }
    }
}