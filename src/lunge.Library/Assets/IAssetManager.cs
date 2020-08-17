using System;

namespace lunge.Library.Assets
{
    public interface IAssetManager : IDisposable
    {
        string RootDirectory { get; set; }
        T Load<T>(string assetName);
    }
}