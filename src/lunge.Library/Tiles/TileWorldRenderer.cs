using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Tiles;

public class TileWorldRenderer : RenderableComponent
{
    public override float Width => World.Width * World.TileWidth;
    public override float Height => World.Height * World.TileHeight;

    public int ActiveLayerIndex { get; set; } = 0;
    
    public int PhysicsLayer = 1 << 0;

    public int TileWidth => World.TileWidth;
    public int TileHeight => World.TileHeight;
    
    public TileWorld World { get; }

    private TileLayer? _collisionLayer;
    private Collider[]? _colliders;

    private int[]? _layersToRender;

    public TileWorldRenderer(TileWorld world, TileLayer? collisionLayer = null)
    {
        World = world;

        _collisionLayer = collisionLayer;
    }

    public override void OnAddedToEntity()
    {
        if (_collisionLayer != null)
        {
            AddColliders(_collisionLayer);
        }
    }

    public void SetLayerToRender(string layer)
    {
        _layersToRender = new int[1];

        _layersToRender[0] = World.Layers.IndexOf(World.GetLayer(layer));
    }

    public void SetLayersToRender(params string[] layers)
    {
        _layersToRender = new int[layers.Length];

        for (int i = 0; i < _layersToRender.Length; i++)
        {
            _layersToRender[i] = World.Layers.IndexOf(World.GetLayer(layers[i]));
        }
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        if (_layersToRender == null)
        {
            foreach (var layer in World.Layers)
                RenderTileLayer(batcher, layer);
        }
        else
        {
            for (int i = 0; i < World.Layers.Count; i++)
            {
                if (World.Layers[i].IsVisible && _layersToRender.Contains(i))
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

                var alpha = 1.0f; //i == ActiveLayerIndex ? 1f : 0.1f;
                var offset = Vector2.Zero; // i == ActiveLayerIndex ? Vector2.Zero : new Vector2(16, 16);

                batcher.DrawSprite(
                    tile.Tile.TileSet.Sprites[tile.Tile.Index],
                    new Vector2(x * TileWidth, y * TileHeight)
                    + offset + Entity.Position,
                    Color.White * alpha
                );
            }
        }
    }
    
    public void AddColliders(TileLayer layer)
    {
        // fetch the collision layer and its rects for collision
        var collisionRects = layer.GetCollisionRectangles();

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
}