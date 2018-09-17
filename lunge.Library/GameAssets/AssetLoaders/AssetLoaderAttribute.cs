using System;
using System.Collections.Generic;

namespace lunge.Library.GameAssets.AssetLoaders
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AssetLoaderAttribute : Attribute
    {
        private readonly List<string> _assetFileExtensions = new List<string>();

        /// <summary>
        /// Friendly asset loader name
        /// </summary>
        public string AssetLoaderName { get; set; }

        public string AssetSubdirectory { get; set; }

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