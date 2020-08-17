using System;
using System.Collections.Generic;

namespace lunge.Library.Assets
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AssetAttribute : Attribute
    {
        private readonly List<string> _assetFileExtensions = new List<string>();

        /// <summary>
        /// Gets or sets friendly asset loader name
        /// </summary>
        public string AssetLoaderName { get; set; }
        
        /// <summary>
        /// Gets all available asset file extensions
        /// </summary>
        public IEnumerable<string> AssetFileExtensions => _assetFileExtensions;

        public AssetAttribute(string assetLoaderName, string assetFileExtension)
        {
            AssetLoaderName = assetLoaderName;
            _assetFileExtensions.Add(assetFileExtension);
        }

        public AssetAttribute(string assetLoaderName, params string[] assetFileExtensions)
        {
            AssetLoaderName = assetLoaderName;
            _assetFileExtensions.AddRange(assetFileExtensions);
        }
    }
}