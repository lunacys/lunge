using Microsoft.Xna.Framework.Content;

namespace lunge.Library.Assets
{
    public class ContentAssetManager : IAssetManager
    {
        public ContentManager Content { get; }
        public string RootDirectory
        {
            get => Content.RootDirectory;
            set => Content.RootDirectory = value;
        }
        public IGame Game { get; }
        
        public ContentAssetManager(IGame game, ContentManager contentManager)
        {
            Game = game;
            Content = contentManager;
        }
        
        public void Dispose()
        {
            Content.Dispose();
        }

        public T Load<T>(string assetName)
        {
            return Content.Load<T>(assetName);
        }

        public T Load<T>(string assetName, string assetType)
        {
            return Content.Load<T>(assetName);
        }

        public T Reload<T>(string assetName, string assetType)
        {
            return Content.Load<T>(assetName);
        }

        public T Reload<T>(string assetName)
        {
            return Content.Load<T>(assetName);
        }

    }
}