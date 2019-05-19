using System;

namespace lunge.Library.GameAssets
{
    public interface IAssetManager : IDisposable
    {
        string AssetDirectory { get; set; }

        T Load<T>(string assetName);
    }
}