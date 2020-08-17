namespace lunge.Library.Assets
{
    public interface IAssetLoader
    {
        IGame Game { get; }
        object Load(string assetName);
    }
    public interface IAssetLoader<out T> : IAssetLoader
    {
        string AssetType { get; }
        new T Load(string assetName);
    }
}