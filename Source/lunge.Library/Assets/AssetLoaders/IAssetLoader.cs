namespace lunge.Library.Assets.AssetLoaders
{
    public interface IAssetLoader<out T>
    {
        T LoadAsset(string assetFilePath);
    }
}