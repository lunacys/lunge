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
        
        public ContentAssetManager(ContentManager contentManager)
        {
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
    }
}