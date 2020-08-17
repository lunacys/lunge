namespace lunge.Library.Assets
{
    public readonly struct AssetNameTypePair
    {
        public string AssetName { get; }
        public string AssetType { get; }

        public AssetNameTypePair(string assetName, string assetType)
        {
            AssetName = assetName;
            AssetType = assetType;
        }
    }
}