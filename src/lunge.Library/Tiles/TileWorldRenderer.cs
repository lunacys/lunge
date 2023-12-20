using System;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Tiles;

public class TileWorldRenderer : RenderableComponent
{
    public override float Width => World.Width * World.TileWidth;
    public override float Height => World.Height * World.TileHeight;
    
    public int PhysicsLayer = 1 << 0;

    public int TileWidth => World.TileWidth;
    public int TileHeight => World.TileHeight;
    
    public TileWorld World { get; }

    private TileLayer? _collisionLayer;

    public TileLayer? CollisionLayer
    {
        get => _collisionLayer;
        set
        {
            _collisionLayer = value;
            RemoveColliders();
            AddColliders();
        }
    }
    
    private Collider[]? _colliders;

    public int[]? LayersToRender;
    
    public TileWorldRenderer(TileWorld world, string? collisionLayer = null)
    {
        World = world;

        _collisionLayer = World.GetLayer(collisionLayer);
    }

    public override void OnEntityTransformChanged(Transform.Component comp)
    {
        if (_collisionLayer != null && comp == Transform.Component.Position)
        {
            RemoveColliders();
            AddColliders();
        }
    }

    public override void OnAddedToEntity()
    {
        AddColliders();
    }

    public override void OnRemovedFromEntity()
    {
        RemoveColliders();
    }

    public void SetLayerToRender(string layer)
    {
        LayersToRender = new int[1];

        LayersToRender[0] = World.Layers.IndexOf(World.GetLayer(layer));
    }

    public void SetLayersToRender(params string[] layers)
    {
        LayersToRender = new int[layers.Length];

        for (int i = 0; i < LayersToRender.Length; i++)
        {
            LayersToRender[i] = World.Layers.IndexOf(World.GetLayer(layers[i]));
        }
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        if (LayersToRender == null)
        {
            for (var i = 0; i < World.Layers.Count; i++)
            {
                if (World.Layers[i].IsVisible)
                    RenderTileLayer(batcher, World.Layers[i]);
            }
        }
        else
        {
            for (int i = 0; i < World.Layers.Count; i++)
            {
                if (World.Layers[i].IsVisible && LayersToRender.Contains(i))
                    RenderTileLayer(batcher, World.Layers[i]);
            }
        }
    }

    public override void DebugRender(Batcher batcher)
    {
        if (_colliders != null)
        {
            foreach (var collider in _colliders)
                collider.DebugRender(batcher);
        }
    }

    public void RenderTileLayer(Batcher batcher, TileLayer layer)
    {
        var w = layer.Width;
        var h = layer.Height;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                var tile = layer[x, y];
                if (tile == null)
                    continue;
                
                batcher.DrawSprite(
                    tile.Tile.TileSet.Sprites[tile.Tile.Index],
                    new Vector2(x * TileWidth, y * TileHeight)
                    + layer.RenderOffset + Entity.Position,
                    Color.White
                );
            }
        }
    }

    public void AddColliders()
    {
        if (_collisionLayer == null)
            return;
        
        // fetch the collision layer and its rects for collision
        var collisionRects = _collisionLayer.GetCollisionRectangles();

        // create colliders for the rects we received
        _colliders = new Collider[collisionRects.Count];
        for (var i = 0; i < collisionRects.Count; i++)
        {
            var collider = new BoxCollider(collisionRects[i].X + _localOffset.X,
                collisionRects[i].Y + _localOffset.Y, collisionRects[i].Width, collisionRects[i].Height);
            collider.PhysicsLayer = PhysicsLayer;
            collider.Entity = Entity;
            _colliders[i] = collider;

            Physics.AddCollider(collider);
        }
    }

    public void RemoveColliders()
    {
        if (_colliders == null)
            return;

        foreach (var collider in _colliders)
            Physics.RemoveCollider(collider);
        _colliders = null;
    }
}