using System;
using Microsoft.Xna.Framework.Content;
using Nez;
using Nez.Systems;

namespace lunge.Library.Assets
{
    public class ContentAssetManager : IAssetManager
    {
        public NezContentManager Content { get; }
        public event EventHandler<AssetReloadedEventArgs> AssetReloaded;
        public string RootDirectory
        {
            get => Content.RootDirectory;
            set => Content.RootDirectory = value;
        }
        
        public ContentAssetManager()
        {
            Content = Core.Content;
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
            var newAsset = Content.Load<T>(assetName);
            
            AssetReloaded?.Invoke(this, new AssetReloadedEventArgs(newAsset, assetName, assetType));
            
            return newAsset;
        }

        public T Reload<T>(string assetName)
        {
            var newAsset = Content.Load<T>(assetName);
            
            AssetReloaded?.Invoke(this, new AssetReloadedEventArgs(newAsset, assetName, null));
            
            return newAsset;
        }

        public object GetAssetByName(string assetName)
        {
            return null;
        }
    }
}