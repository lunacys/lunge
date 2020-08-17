namespace lunge.Library.Assets
{
    public class HotReloadAssetManager : IAssetManager
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public string RootDirectory { get; set; }
        public T Load<T>(string assetName)
        {
            throw new System.NotImplementedException();
        }
    }
}