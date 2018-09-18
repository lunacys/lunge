using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using lunge.Library.GameAssets.AssetLoaders;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.GameAssets
{
    /// <summary>
    /// Represents an asset loader that can load raw (not converted to .xnb format) files.
    /// </summary>
    public class AssetManager
    {
        private readonly Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();
        private readonly Dictionary<string, object> _assetLoaders = new Dictionary<string, object>();

        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Gets or sets asset directory which is currently in use
        /// </summary>
        public string AssetDirectory { get; set; }

        public AssetManager(GraphicsDevice graphicsDevice, string assetDirectory = "Content")
        {
            _graphicsDevice = graphicsDevice;

            AssetDirectory = assetDirectory;
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

            // Get current assembly
            var loaderAssembly = Assembly.GetExecutingAssembly();

            //Console.WriteLine($"Name: {loaderAssembly.FullName}");

            // Get first class than implements IAssetLoader interface with generic type T
            var type = loaderAssembly
                .GetTypes()
                .FirstOrDefault(t => t.IsClass && t.GetInterfaces().Any(x =>
                                         x.IsGenericType &&
                                         x.GetGenericTypeDefinition() == typeof(IAssetLoader<>) &&
                                         x.GetGenericArguments()[0] == typeof(T)));

            if (type == null) throw new InvalidOperationException("Cannot find loader for this asset type");

            //Console.WriteLine($"Full Name: {type.FullName}");

            string assetLoaderName = String.Empty;
            string assetSubdirectory = null;
            IEnumerable<string> assetFileExtensions = new List<string>();

            var attribute = type.GetCustomAttributes(false).FirstOrDefault(a => a is AssetLoaderAttribute);
            if (attribute is AssetLoaderAttribute a1)
            {
                assetLoaderName = a1.AssetLoaderName;
                assetSubdirectory = a1.AssetSubdirectory;
                assetFileExtensions = a1.AssetFileExtensions;
            }

            IAssetLoader<T> assetLoader;

            // Optimization: if we've already used this asset loader we do not need to create instance of it again
            if (_assetLoaders.ContainsKey(assetLoaderName))
            {
                assetLoader = (IAssetLoader<T>)_assetLoaders[assetLoaderName];
            }
            else
            {
                // If it is a graphic asset, we should set its GraphicsDevice property
                if (type.GetInterfaces().Any(x => x == typeof(IGraphicalAsset)))
                {
                    var graphAsset = (IGraphicalAsset)Activator.CreateInstance(type);
                    graphAsset.GraphicsDevice = _graphicsDevice;

                    assetLoader = graphAsset as IAssetLoader<T>;
                }
                else
                {
                    assetLoader = (IAssetLoader<T>)Activator.CreateInstance(type);
                }

                _assetLoaders[assetLoaderName] = assetLoader;
            }

            // Pass through the Load method 
            var asset = Load(assetName, assetLoader, assetSubdirectory, assetFileExtensions);

            return asset;
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
    }
}