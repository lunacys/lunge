using System;

namespace lunge.Library.Assets
{
    public interface IAssetWatcher
    {
        event EventHandler<AssetReloadedEventArgs> AssetChanged;
        IAssetManager AssetManager { get; }
        void Watch(string assetName);
        void Watch(string assetName, string assetType);
    }
}