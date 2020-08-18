﻿using System;
using Microsoft.Xna.Framework.Content;

namespace lunge.Library.Assets
{
    public class ContentAssetManager : IAssetManager
    {
        public ContentManager Content { get; }
        public event EventHandler<AssetReloadedEventArgs> AssetReloaded;
        public string RootDirectory
        {
            get => Content.RootDirectory;
            set => Content.RootDirectory = value;
        }
        public IGame Game { get; }
        
        public ContentAssetManager(IGame game, ContentManager contentManager)
        {
            Game = game;
            Content = contentManager;
        }
        
        public void Dispose()
        {
            Content.Dispose();
        }

        public T Load<T>(string assetName)
        {
            return Content.Load<T>(assetName);
        }

        public T Load<T>(string assetName, string assetType)
        {
            return Content.Load<T>(assetName);
        }

        public T Reload<T>(string assetName, string assetType)
        {
            var newAsset = Content.Load<T>(assetName);
            
            AssetReloaded?.Invoke(this, new AssetReloadedEventArgs(newAsset, assetName, assetType));
            
            return newAsset;
        }

        public T Reload<T>(string assetName)
        {
            var newAsset = Content.Load<T>(assetName);
            
            AssetReloaded?.Invoke(this, new AssetReloadedEventArgs(newAsset, assetName, null));
            
            return newAsset;
        }

        public object GetAssetByName(string assetName)
        {
            return null;
        }
    }
}