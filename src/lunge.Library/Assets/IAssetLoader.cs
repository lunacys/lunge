namespace lunge.Library.Assets
{
    public interface IAssetLoader
    {
        object Load(string assetName);
    }
    public interface IAssetLoader<out T> : IAssetLoader
    {
        string AssetType { get; }
        new T Load(string assetName);
    }
}