﻿namespace lunge.Library.GameAssets.AssetLoaders
{
    public interface IAssetLoader<out T>
    {
        T LoadAsset(string assetFilePath);
    }
}