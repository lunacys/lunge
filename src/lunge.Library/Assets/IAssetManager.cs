using System;

namespace lunge.Library.Assets
{
    public interface IAssetManager : IDisposable
    {
        IGame Game { get; }
        string RootDirectory { get; set; }
        event EventHandler<AssetReloadedEventArgs> AssetReloaded;
        T Load<T>(string assetName);
        T Load<T>(string assetName, string assetType);
        T Reload<T>(string assetName, string assetType);
        T Reload<T>(string assetName);
        object GetAssetByName(string assetName);
    }
}