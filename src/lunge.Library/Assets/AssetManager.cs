using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using lunge.Library.Assets.AssetLoaders;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Assets
{
    /// <summary>
    /// Represents an asset loader that can load raw (not converted to .xnb format) files.
    /// </summary>
    public class AssetManager : IAssetManager
    {
        private readonly Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();
        private readonly List<Tuple<object, AssetLoaderAttribute>> _assetLoaders = new List<Tuple<object, AssetLoaderAttribute>>();

        /// <summary>
        /// Gets or sets asset directory which is currently in use
        /// </summary>
        public string AssetDirectory { get; set; }

        public AssetManager(string assetDirectory = "Content")
        {
            AssetDirectory = assetDirectory;

            InitializeContentLoaders();
        }

        /// <summary>
        /// Gets a loaded asset by name. If there is no asset with this name, returns null.
        /// </summary>
        /// <param name="name">Asset name</param>
        /// <returns>Asset object</returns>
        public object this[string name]
        {
            get
            {
                if (_loadedAssets.ContainsKey(name))
                    return _loadedAssets[name];
                return null;
            }
        }

        private void InitializeContentLoaders()
        {
            // if (_graphicsDevice == null)
            //     throw new NullReferenceException("Please initialize GraphicsDevice first");

            // Get current assembly
            var loaderAssembly = Assembly.GetExecutingAssembly();

            // Find all content loaders
            var types = loaderAssembly
                .GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Any(x =>
                    x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAssetLoader<>)));

            // TODO: Add a better way to handle and load assets
            foreach (var type in types)
            {
                if (type
                    .GetCustomAttributes(false)
                    .FirstOrDefault(a => a is AssetLoaderAttribute) is AssetLoaderAttribute assetLoaderAttribute)
                {
                    /*if (type.GetInterfaces().Any(x => x == typeof(IGraphicalAsset)))
                    {
                        var graphAsset = (IGraphicalAsset)Activator.CreateInstance(type);
                        graphAsset.GraphicsDevice = _graphicsDevice;
                        _assetLoaders.Add(new Tuple<object, AssetLoaderAttribute>(graphAsset, assetLoaderAttribute));
                    }
                    else
                    {*/
                    var assetLoader = Activator.CreateInstance(type);
                    _assetLoaders.Add(new Tuple<object, AssetLoaderAttribute>(assetLoader, assetLoaderAttribute));
                    // }
                }
            }
        }

        /// <summary>
        /// Loads and returns an asset from the <see cref="AssetDirectory"/>
        /// </summary>
        /// <typeparam name="T">Type of asset</typeparam>
        /// <param name="assetName">Asset name</param>
        /// <returns>The asset</returns>
        public virtual T Load<T>(string assetName)
        {
            if (_loadedAssets.ContainsKey(assetName))
                return (T)_loadedAssets[assetName];

            var loader = _assetLoaders.FirstOrDefault(o => o.Item1 is IAssetLoader<T>);

            if (loader == null)
                throw new NullReferenceException("Couldn't find asset loader for specified type");

            return Load(assetName, loader.Item1 as IAssetLoader<T>, loader.Item2.AssetSubdirectory, loader.Item2.AssetFileExtensions);
        }

        private T Load<T>(string assetName,
            IAssetLoader<T> assetLoader,
            string assetSubdirectory,
            IEnumerable<string> assetFileExtensions)
        {
            var filepath =
                (assetSubdirectory != null) // if subdir is null then the asset is placed in the root of the content directory
                    ? (Path.Combine(AssetDirectory, assetSubdirectory, assetName))
                    : (Path.Combine(AssetDirectory, assetName));

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"Unable to find specified file: {filepath}");

            // find if extension of the file is supported
            if (assetFileExtensions.All(s => Path.GetExtension(filepath) != s))
                throw new Exception("Cannot load an asset with this extension");

            var asset = assetLoader.LoadAsset(filepath);
            _loadedAssets.Add(assetName, asset);

            return asset;
        }

        public void Dispose()
        {
            // _graphicsDevice.Dispose();
        }
    }
}