using System;
using System.Collections.Generic;

namespace lunge.Library.GameAssets.AssetLoaders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AssetLoaderAttribute : Attribute
    {
        private readonly List<string> _assetFileExtensions = new List<string>();

        /// <summary>
        /// Gets or sets friendly asset loader name
        /// </summary>
        public string AssetLoaderName { get; set; }

        /// <summary>
        /// Gets or sets asset subdirectory. Use null value if asset must not be in a subdir
        /// </summary>
        public string AssetSubdirectory { get; set; }

        /// <summary>
        /// Gets all available asset file extensions
        /// </summary>
        public IEnumerable<string> AssetFileExtensions => _assetFileExtensions;

        public AssetLoaderAttribute(string assetLoaderName, string assetSubdirectory, string assetFileExtension)
        {
            AssetLoaderName = assetLoaderName;
            AssetSubdirectory = assetSubdirectory;
            _assetFileExtensions.Add(assetFileExtension);
        }

        public AssetLoaderAttribute(string assetLoaderName, string assetSubdirectory, params string[] assetFileExtensions)
        {
            AssetLoaderName = assetLoaderName;
            AssetSubdirectory = assetSubdirectory;
            _assetFileExtensions.AddRange(assetFileExtensions);
        }
    }
}