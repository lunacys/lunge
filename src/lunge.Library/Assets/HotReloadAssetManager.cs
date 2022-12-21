using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace lunge.Library.Assets
{
    public class HotReloadAssetManager : IAssetManager
    {
        private readonly Dictionary<string, object> _loadedAssets;
        private readonly Dictionary<AssetNameTypePair, object> _loadedAssetsByType;
        private readonly List<(IAssetLoader, AssetAttribute)> _assetLoaders;

        public static string GeneralAssetTypeName { get; set; }
        
        public string RootDirectory { get; set; }

        public event EventHandler<AssetReloadedEventArgs> AssetReloaded;

        public HotReloadAssetManager()
        {
            GeneralAssetTypeName = "General";
            
            _loadedAssets = new Dictionary<string, object>();
            _loadedAssetsByType = new Dictionary<AssetNameTypePair, object>();
            _assetLoaders = new List<(IAssetLoader, AssetAttribute)>();
            
            InitializeAssetLoaders();
        }
        
        public void Dispose()
        {
            foreach (var asset in _loadedAssets.Values)
            {
                if (asset is IDisposable disposable)
                    disposable.Dispose();
            }
        }
        
        /// <summary>
        /// Loads an asset with specified asset name. File path must be included in the asset name. If asset with that name was loaded previously, it takes it from cache.
        /// </summary>
        /// <typeparam name="T">Type of the required asset</typeparam>
        /// <param name="assetName">Name of the asset to be loaded</param>
        /// <returns>Loaded asset</returns>
        public T Load<T>(string assetName)
        {
            if (_loadedAssets.ContainsKey(assetName))
                return (T) _loadedAssets[assetName];
            
            return Load<T>(assetName, GeneralAssetTypeName);
        }

        /// <summary>
        /// Loads an asset with specified asset name and type. File path must be included in the asset name.
        /// </summary>
        /// <typeparam name="T">Type of the required asset</typeparam>
        /// <param name="assetName">Name of the asset to be loaded</param>
        /// <param name="assetType">Type of the asset</param>
        /// <returns>Loaded asset</returns>
        public T Load<T>(string assetName, string assetType)
        {
            var nameTypePair = new AssetNameTypePair(assetName, assetType);
            
            if (_loadedAssetsByType.ContainsKey(nameTypePair))
                return (T) _loadedAssetsByType[nameTypePair];

            if (assetType == GeneralAssetTypeName)
            {
                return (T) LoadGeneralFile(assetName);
            }

            var assetLoader = _assetLoaders.Find(tuple =>
            {
                var genericAssetLoader = tuple.Item1 as IAssetLoader<T>;
                if (genericAssetLoader == null)
                    return false;

                return genericAssetLoader.AssetType == assetType;
            });
            
            if (assetLoader == (null, null))
                throw new Exception($"Could not find asset loader for type '{assetType}'");

            return LoadInternal<T>(assetName, assetType, assetLoader.Item1 as IAssetLoader<T>,
                assetLoader.Item2.AssetFileExtensions);
        }

        public T Reload<T>(string assetName, string assetType)
        {
            var nameTypePair = new AssetNameTypePair(assetName, assetType);
            
            var asset = _loadedAssetsByType[nameTypePair];

            if (asset == null)
                throw new Exception($"No asset found with name '{assetName}' and type '{assetType}'");
            
            if (asset is IDisposable disposable)
                disposable.Dispose();

            _loadedAssets.Remove(assetName);
            _loadedAssetsByType.Remove(nameTypePair);

            var newAsset = Load<T>(assetName, assetType);

            AssetReloaded?.Invoke(this, new AssetReloadedEventArgs(newAsset, assetName, assetType));

            return newAsset;
        }
        
        public T Reload<T>(string assetName)
        {
            var asset = _loadedAssets[assetName];

            if (asset == null)
                throw new Exception($"No asset found with name '{assetName}'");
            
            if (asset is IDisposable disposable)
                disposable.Dispose();

            _loadedAssets.Remove(assetName);
            _loadedAssetsByType.Remove(new AssetNameTypePair(assetName, GeneralAssetTypeName));

            var newAsset = Load<T>(assetName);
            
            AssetReloaded?.Invoke(this, new AssetReloadedEventArgs(newAsset, assetName, GeneralAssetTypeName));

            return newAsset;
        }

        public object GetAssetByName(string assetName)
        {
            return _loadedAssets[assetName];
        }

        private void InitializeAssetLoaders()
        {
            var loaderAssembly = Assembly.GetExecutingAssembly();
            var entryAssembly = Assembly.GetEntryAssembly();

            // Check all available classes that implement IAssetLoader interface
            var types = loaderAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Any(x =>
                    x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAssetLoader<>)));
                
            if (entryAssembly != null)
            {
                types = types.Concat(
                    entryAssembly.GetTypes()
                        .Where(t => t.IsClass && t.GetInterfaces()
                            .Any(x => x.IsGenericType &&
                                      x.GetGenericTypeDefinition() == typeof(IAssetLoader<>))));
            }

            // Go though them and find those that use AssetAttribute
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<AssetAttribute>();
                if (attribute != null)
                {
                    var assetLoader = Activator.CreateInstance(type);
                    _assetLoaders.Add((assetLoader as IAssetLoader, attribute));
                }
            }
        }

        private T LoadInternal<T>(
            string assetName, 
            string assetType,
            IAssetLoader<T> assetLoader,
            IEnumerable<string> allowedFileExtensions)
        {
            var filepath= Path.Combine(RootDirectory, assetName);

            filepath = GetFullFilePathWithExtension(filepath, allowedFileExtensions);

            var asset = assetLoader.Load(filepath);
            _loadedAssetsByType[new AssetNameTypePair(assetName, assetType)] = asset;
            _loadedAssets.Add(assetName, asset);

            return asset;
        }

        private string GetFullFilePathWithExtension(string assetName, IEnumerable<string> extensions)
        {
            foreach (var extension in extensions)
            {
                var fileWithExt = assetName + extension;
                
                if (File.Exists(fileWithExt))
                    return fileWithExt;
            }
            
            throw new FileNotFoundException($"The specific asset '{assetName}' was not found");
        }

        private object LoadGeneralFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();
            
            // TODO: Binary or text?
            // Loading as string
            using (var sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }
    }
}