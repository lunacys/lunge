using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lunge.Library.Assets
{
    public class AssetWatcher : IAssetWatcher, IDisposable
    {
        public event EventHandler<AssetReloadedEventArgs> AssetChanged;
        public IAssetManager AssetManager { get; }
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly List<AssetNameTypePair> _watchedAssets;
        

        public AssetWatcher(IAssetManager assetManager)
        {
            AssetManager = assetManager;
            _watchedAssets = new List<AssetNameTypePair>();
            _fileSystemWatcher = new FileSystemWatcher(AssetManager.RootDirectory);
            AssetManager.AssetReloaded += AssetManagerOnAssetReloaded;
            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;
        }

        public void Watch(string assetName)
        {
            _watchedAssets.Add(new AssetNameTypePair(assetName, HotReloadAssetManager.GeneralAssetTypeName));
        }

        public void Watch(string assetName, string assetType)
        {
            _watchedAssets.Add(new AssetNameTypePair(assetName, assetType));
        }

        public void Dispose()
        {
            _fileSystemWatcher.Dispose();
        }
        
        private void AssetManagerOnAssetReloaded(object sender, AssetReloadedEventArgs e)
        {
            var nameTypePair = new AssetNameTypePair(e.AssetName, e.AssetType);
            if (_watchedAssets.Contains(nameTypePair))
            {
                AssetChanged?.Invoke(this, e);
            }
        }
        
        private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            var assetName = Path.GetFileNameWithoutExtension(e.FullPath);

            Console.WriteLine($"Changed: {e.FullPath}, {e.Name}, {assetName}");

            var existingAsset = _watchedAssets.Find(pair => pair.AssetName.StartsWith(assetName));
            
            if (existingAsset.AssetName != null && existingAsset.AssetType != null)
            {
                AssetChanged?.Invoke(this,
                    new AssetReloadedEventArgs(AssetManager.GetAssetByName(existingAsset.AssetName), existingAsset.AssetName, null));
            }
        }
    }
}