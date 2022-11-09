using System;
using System.Collections.Generic;
using System.IO;
using Nez;
using Nez.Persistence;
using Nez.Sprites;
using Nez.Systems;
using Nez.Textures;

namespace lunge.Library.Assets.Aseprite;

public static class AsepriteLoader
{
    private static Dictionary<string, AsepriteAnimationData> _cache = new();

    /// <summary>
    /// Loads raw Aseprite animation data in format Aseprite saves it.
    /// </summary>
    /// <param name="filename">*.json file</param>
    /// <returns></returns>
    public static AsepriteAnimationData LoadRaw(string filename)
    {
        if (_cache.ContainsKey(filename))
            return _cache[filename];

        var fileContent = File.ReadAllText(filename);

        var obj = Json.FromJson<AsepriteAnimationData>(fileContent);
        _cache[filename] = obj;

        return obj;
    }

    public static void Unload(string filename)
    {
        if (_cache.ContainsKey(filename))
            _cache.Remove(filename);
    }

    public static void UnloadAll()
    {
        _cache.Clear();
    }

    public static SpriteAnimator BuildAnimator(NezContentManager content, string filename, bool premultiplyAlpha = true)
    {
        var data = LoadRaw(filename);

        var currDir = Path.GetDirectoryName(filename);
        if (currDir == null)
            throw new Exception($"Current Dir is null ({filename})");
        var imagePath = Path.Combine(currDir, data.Meta.Image);
        var texture = content.LoadTexture(imagePath, premultiplyAlpha);

        //var firstFrame = data.Frames[0];
        //var sprites = Sprite.SpritesFromAtlas(texture, firstFrame.Frame.W, firstFrame.Frame.H);

        var animator = new SpriteAnimator();
        foreach (var tag in data.Meta.FrameTags)
        {
            var frameSprites = new Sprite[tag.To - tag.From + 1];
            var fps = 1000f / data.Frames[tag.From].Duration;

            for (int i = tag.From; i <= tag.To; i++)
            {
                var frameData = data.Frames[i].Frame;
                frameSprites[i - tag.From] = new Sprite(texture, frameData.X, frameData.Y, frameData.W, frameData.H);
            }

            animator.AddAnimation(tag.Name, frameSprites, fps);
        }

        return animator;
    }

    /// <summary>
    /// Builds and returns an Entity that contains all the components needed for animation handling.
    /// NOTE: This method does not support different frame sizes within an animation.
    /// </summary>
    /// <param name="content">Nez Content Manager</param>
    /// <param name="filename">*.json file</param>
    /// <param name="entityName">Name with which entity will be created and returned</param>
    /// <returns></returns>
    public static Entity BuildEntity(NezContentManager content, string filename, string entityName)
    {
        var data = LoadRaw(filename);

        var entity = new Entity(entityName); 

        // Step 1: Load Sprite Atlas texture
        var currDir = Path.GetDirectoryName(filename);
        var texture = content.LoadTexture($"{currDir}/{data.Meta.Image}", true);

        if (data.Frames == null || data.Frames.Length == 0)
        {
            entity.AddComponent(new SpriteRenderer(texture));
            return entity;
        }

        // Step 2: Create Sprites from the Atlas
        var firstFrame = data.Frames[0];
        var sprites = Sprite.SpritesFromAtlas(texture, firstFrame.Frame.W, firstFrame.Frame.H);

        // Step 3: Add SpriteAnimator Component
        var animator = entity.AddComponent(new SpriteAnimator());

        // Step 4: Add all animation states
        foreach (var tag in data.Meta.FrameTags)
        {
            var frameSprites = new Sprite[tag.To - tag.From + 1];
            var fps = 1f / data.Frames[tag.From].Duration;
            
            for (int i = tag.From; i <= tag.To; i++)
            {
                frameSprites[i - tag.From] = sprites[i];
            }

            animator.AddAnimation(tag.Name, frameSprites, fps);
        }

        return entity;
    }
}