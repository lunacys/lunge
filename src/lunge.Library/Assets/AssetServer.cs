using System;
using System.IO;

namespace lunge.Library.Assets
{
    public class AssetServer : IAssetServer, IDisposable
    {
        private FileSystemWatcher _fileWatcher;
        
        public IAssetManager AssetManager { get; }

        public AssetServer(IAssetManager assetManager)
        {
            AssetManager = assetManager;
        }

        public void Run()
        {
            if (_fileWatcher != null) 
                throw new Exception("Server is running already");
            
            _fileWatcher = new FileSystemWatcher(AssetManager.AssetDirectory);
            _fileWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_fileWatcher == null)
                throw new Exception("Server is not running");
            
            _fileWatcher.Dispose();
        }

        public void Dispose()
        {
            _fileWatcher.Dispose();
        }
    }
}