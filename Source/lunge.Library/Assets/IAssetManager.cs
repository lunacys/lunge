using System;

namespace lunge.Library.Assets
{
    public interface IAssetManager : IDisposable
    {
        string AssetDirectory { get; set; }

        T Load<T>(string assetName);
    }
}