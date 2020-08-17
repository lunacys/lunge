using System;

namespace lunge.Library.Assets
{
    public interface IAssetManager : IDisposable
    {
        IGame Game { get; }
        string RootDirectory { get; set; }
        T Load<T>(string assetName);
        T Load<T>(string assetName, string assetType);
        T Reload<T>(string assetName, string assetType);
        T Reload<T>(string assetName);
    }
}