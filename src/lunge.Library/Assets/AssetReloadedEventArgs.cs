using System;

namespace lunge.Library.Assets
{
    public class AssetReloadedEventArgs : EventArgs
    {
        public object NewAsset { get; }
        public string AssetName { get; }
        public string AssetType { get; }
        
        public AssetReloadedEventArgs(object newAsset, string assetName, string assetType)
        {
            NewAsset = newAsset;
            AssetName = assetName;
            AssetType = assetType;
        }
    }
}