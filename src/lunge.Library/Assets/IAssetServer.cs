namespace lunge.Library.Assets
{
    public interface IAssetServer
    {
        IAssetManager AssetManager { get; }
        void Run();
        void Stop();
    }
}