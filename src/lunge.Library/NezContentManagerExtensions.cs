using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Nez.BitmapFonts;
using Nez.Systems;
using Nez.Tiled;

namespace lunge.Library;

public static class NezContentManagerExtensions
{
   /// <summary>
   /// loads a Texture2D on a background thread with optional callback that includes a context parameter for when it is loaded.
   /// The callback will occur on the main thread.
   /// </summary>
   /// <param name="assetName">Asset name.</param>
   /// <param name="onLoaded">On loaded.</param>
   public static void LoadTextureAsync(this NezContentManager content, string assetName, bool premultiplyAlpha = false, Action<Texture2D>? onLoaded = null)
   {
      var syncContext = SynchronizationContext.Current;
      Task.Run(() =>
      {
         var asset = content.LoadTexture(assetName, premultiplyAlpha);

         // if we have a callback do it on the main thread
         if (onLoaded != null)
         {
            syncContext?.Post(d => { onLoaded(asset); }, null);
         }
      });
   }
   
   /// <summary>
   /// loads an asset on a background thread with optional callback that includes a context parameter for when it is loaded.
   /// The callback will occur on the main thread.
   /// </summary>
   /// <param name="assetName">Asset name.</param>
   /// <param name="onLoaded">On loaded.</param>
   /// <param name="context">Context.</param>
   public static void LoadTextureAsync(this NezContentManager content, string assetName, bool premultiplyAlpha = false, Action<object?, Texture2D>? onLoaded = null, object? context = null)
   {
      var syncContext = SynchronizationContext.Current;
      Task.Run(() =>
      {
         var asset = content.LoadTexture(assetName, premultiplyAlpha);

         if (onLoaded != null)
         {
            syncContext?.Post(d => { onLoaded(context, asset); }, null);
         }
      });
   }
   
   /// <summary>
   /// loads an asset on a background thread with optional callback that includes a context parameter for when it is loaded.
   /// The callback will occur on the main thread.
   /// </summary>
   /// <param name="assetName">Asset name.</param>
   /// <param name="onLoaded">On loaded.</param>
   public static void LoadBitmapFontAsync(this NezContentManager content, string assetName, bool premultiplyAlpha = false, Action<BitmapFont>? onLoaded = null)
   {
      var syncContext = SynchronizationContext.Current;
      Task.Run(() =>
      {
         var asset = content.LoadBitmapFont(assetName, premultiplyAlpha);

         // if we have a callback do it on the main thread
         if (onLoaded != null)
         {
            syncContext?.Post(d => { onLoaded(asset); }, null);
         }
      });
   }
   
   /// <summary>
   /// loads an asset on a background thread with optional callback that includes a context parameter for when it is loaded.
   /// The callback will occur on the main thread.
   /// </summary>
   /// <param name="assetName">Asset name.</param>
   /// <param name="onLoaded">On loaded.</param>
   /// <param name="context">Context.</param>
   public static void LoadBitmapFontAsync(this NezContentManager content, string assetName, bool premultiplyAlpha = false, Action<object?, BitmapFont>? onLoaded = null, object? context = null)
   {
      var syncContext = SynchronizationContext.Current;
      Task.Run(() =>
      {
         var asset = content.LoadBitmapFont(assetName, premultiplyAlpha);

         if (onLoaded != null)
         {
            syncContext?.Post(d => { onLoaded(context, asset); }, null);
         }
      });
   }
   
   /// <summary>
   /// loads an asset on a background thread with optional callback that includes a context parameter for when it is loaded.
   /// The callback will occur on the main thread.
   /// </summary>
   /// <param name="assetName">Asset name.</param>
   /// <param name="onLoaded">On loaded.</param>
   public static void LoadTiledMapAsync(this NezContentManager content, string assetName, Action<TmxMap>? onLoaded = null)
   {
      var syncContext = SynchronizationContext.Current;
      Task.Run(() =>
      {
         var asset = content.LoadTiledMap(assetName);

         // if we have a callback do it on the main thread
         if (onLoaded != null)
         {
            syncContext?.Post(d => { onLoaded(asset); }, null);
         }
      });
   }
   
   /// <summary>
   /// loads an asset on a background thread with optional callback that includes a context parameter for when it is loaded.
   /// The callback will occur on the main thread.
   /// </summary>
   /// <param name="assetName">Asset name.</param>
   /// <param name="onLoaded">On loaded.</param>
   /// <param name="context">Context.</param>
   public static void LoadTiledMapAsync(this NezContentManager content, string assetName, Action<object?, TmxMap>? onLoaded = null, object? context = null)
   {
      var syncContext = SynchronizationContext.Current;
      Task.Run(() =>
      {
         var asset = content.LoadTiledMap(assetName);

         if (onLoaded != null)
         {
            syncContext?.Post(d => { onLoaded(context, asset); }, null);
         }
      });
   }
}