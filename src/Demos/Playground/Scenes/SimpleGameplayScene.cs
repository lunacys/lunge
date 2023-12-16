using lunge.Library.Assets.Aseprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Playground.Components;
using Playground.Components.Renderers;

namespace Playground.Scenes;

public class SimpleGameplayScene : Scene
{
    public const int ScreenSpaceRenderLayer = 999;

    public static readonly int Width = 1920;
    public static readonly int Height = 1200;

    public const int LightLayer = 5;
    public const int LightMapLayer = 10;
    public const int BgLayer = 15;

    public SimpleGameplayScene()
    {
        var renderer = AddRenderer(new ScreenSpaceRenderer(100,
            ScreenSpaceRenderLayer));

        // AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));


    }

    public override void Initialize()
    {
        base.Initialize();

        SetDesignResolution(2560, 1440, SceneResolutionPolicy.ShowAll);
        Screen.SetSize(2560, 1440);

        /*SetDesignResolution(1280, 720, SceneResolutionPolicy.ShowAll);
        Screen.SetSize(1280, 720);*/

        var raw = AsepriteLoader.LoadRaw("Content/characters/ss1.json");
        var ent = AsepriteLoader.BuildEntity(Content, "Content/characters/ss1.json", "my-ent");
        var temp = System.IO.Path.GetTempPath();
        ClearColor = Color.White;

        //Core.Instance.Window.IsBorderless = true;

        // CreateEntity("profiler").AddComponent(new ProfilerComponent()).AddComponent(new LoggerComponent());

        var lightRenderer = AddRenderer(new StencilLightRenderer(-1, LightLayer, new RenderTexture()));
        lightRenderer.RenderTargetClearColor = new Color(20, 20, 20, 255);

        AddRenderer(new RenderLayerRenderer(0, BgLayer));
        AddRenderer(new RenderLayerRenderer(1, LightMapLayer));

        AddRenderer(new RenderLayerRenderer(2, 0));
        Camera.RawZoom = 4;
        // Add Lights
        var lightTex = Content.LoadTexture(Nez.Content.Spritelight);
        CreateEntity("light-texture")
            .SetPosition(new Vector2(200, 200))
            .SetScale(8)
            .AddComponent(new SpriteRenderer(lightTex))
            .SetRenderLayer(LightLayer)
            .SetColor(Color.GreenYellow);

        var l1 = CreateEntity("light")
            .SetPosition(new Vector2(100, 100))
            .SetScale(4);
        l1.AddComponent(new StencilLight(100, Color.White))
            .SetRenderLayer(LightLayer);
        l1.AddComponent(new StencilLightScaleHandler());
        var l2 = CreateEntity("light2")
            .SetPosition(new Vector2(800, 400))
            .SetScale(4);
        l2.AddComponent(new StencilLight(200, Color.AntiqueWhite))
            .SetRenderLayer(LightLayer);
        l2.AddComponent(new StencilLightScaleHandler());

        // Add Props
        /*var propTex = Content.LoadTexture(Nez.Content.Steering.YellowArrow);
        CreateEntity("prop1")
            .SetPosition(new Vector2(64, 64))
            .AddComponent(new SpriteRenderer(propTex));*/

        var tiledEntity = CreateEntity("tiled-map-entity");
        var map = Content.LoadTiledMap(Nez.Content.Map.Tilemap);
        var tiledMapRenderer = tiledEntity.AddComponent(new TiledMapRenderer(map, "collision"));
        tiledMapRenderer.SetLayersToRender(new[] { "tiles", "terrain", "details" });

        tiledMapRenderer.RenderLayer = BgLayer;

        var tiledMapDetailsComp = tiledEntity.AddComponent(new TiledMapRenderer(map));
        tiledMapDetailsComp.SetLayersToRender("above-details");
        tiledMapDetailsComp.RenderLayer = -1;

        tiledMapDetailsComp.Material = Material.StencilWrite();
        tiledMapDetailsComp.Material.Effect = Content.LoadNezEffect<SpriteAlphaTestEffect>();

        var topLeft = new Vector2(map.TileWidth, map.TileWidth);
        var bottomRight = new Vector2(map.TileWidth * (map.Width - 1),
            map.TileWidth * (map.Height - 1));
        tiledEntity.AddComponent(new CameraBounds(topLeft, bottomRight));


        var playerEntity = CreateEntity("player", new Vector2(256 / 2, 224 / 2));
        playerEntity.AddComponent(new PlayerComponent());
        var collider = playerEntity.AddComponent<CircleCollider>();

        // we only want to collide with the tilemap, which is on the default layer 0
        Flags.SetFlagExclusive(ref collider.CollidesWithLayers, 0);

        // move ourself to layer 1 so that we dont get hit by the projectiles that we fire
        Flags.SetFlagExclusive(ref collider.PhysicsLayer, 1);
        
        Camera.Entity.AddComponent(new FollowCamera(playerEntity));


        /*var bgTexture = Content.LoadTexture(Nez.Content.Bg);
        CreateEntity("bg")
            .SetPosition(Screen.Center)
            .SetScale(10)
            .AddComponent(new SpriteRenderer(bgTexture))
            .SetRenderLayer(BgLayer);*/

        _lightMapEntity = CreateEntity("light-map")
            .SetPosition(Screen.Center);
            //.SetScale(1 / 4f)
        _lightMapEntity.AddComponent(new SpriteRenderer(lightRenderer.RenderTexture))
            //.SetOrigin(Vector2.Zero)
            .SetMaterial(Material.BlendMultiply())
            .SetRenderLayer(LightMapLayer);
        _lightMapEntity.AddComponent(new StencilLightMapHandler());
    }

    private Entity _lightMapEntity;

    public Entity CreateProjectiles(Vector2 position, Vector2 velocity)
    {
        // create an Entity to house the projectile and its logic
        var entity = CreateEntity("projectile");
        entity.Position = position;
        entity.AddComponent(new ProjectileMover());
        entity.AddComponent(new FireballProjectileController(velocity));

        // add a collider so we can detect intersections
        var collider = entity.AddComponent<CircleCollider>();
        Flags.SetFlagExclusive(ref collider.CollidesWithLayers, 0);
        Flags.SetFlagExclusive(ref collider.PhysicsLayer, 1);


        // load up a Texture that contains a fireball animation and setup the animation frames
        var texture = Content.LoadTexture(Nez.Content.Plume);
        var sprites = Sprite.SpritesFromAtlas(texture, 16, 16);

        // add the Sprite to the Entity and play the animation after creating it
        var animator = entity.AddComponent(new SpriteAnimator());

        // render after (under) our player who is on renderLayer 0, the default
        animator.RenderLayer = 1;

        animator.AddAnimation("default", sprites.ToArray());
        animator.Play("default");

        // clone the projectile and fire it off in the opposite direction
        var newEntity = entity.Clone(entity.Position);
        newEntity.GetComponent<FireballProjectileController>().Velocity *= -1;
        AddEntity(newEntity);

        return entity;
    }

    public override void Update()
    {
        if (Input.IsKeyDown(Keys.W))
            Camera.Position -= Vector2.UnitY * 4;
        if (Input.IsKeyDown(Keys.S))
            Camera.Position += Vector2.UnitY * 4;
        if (Input.IsKeyDown(Keys.A))
            Camera.Position -= Vector2.UnitX * 4;
        if (Input.IsKeyDown(Keys.D))
            Camera.Position += Vector2.UnitX * 4;

        if (Input.IsKeyDown(Keys.Z))
            Camera.ZoomIn(0.1f);
        if (Input.IsKeyDown(Keys.X))
            Camera.ZoomOut(0.1f);

        

        base.Update();
    }
}