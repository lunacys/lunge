using lunge.Library.AI.Pathfinding;
using lunge.Library.Assets;
using lunge.Library.Debugging.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez3D;
using Playground2.Components.Pathfinding2;

namespace Playground2.Scenes
{
    public class PathfindingScene : Scene
    {
        public const int ScreenSpaceRenderLayer = 999;

        const int RenderablesLayer = 5;
        const int LightLayer = 10;

        public const int Width = 1920;
        public const int Height = 1080;

        private AssetWatcher _assetWatcher;

        private readonly ILogger _logger = LoggerFactory.GetLogger("PathfindingScene");

        public PathfindingScene()
        {
            _logger.Debug("Constructing, adding screen space renderer");

            AddRenderer(new ScreenSpaceRenderer(100,
                ScreenSpaceRenderLayer));

            _logger.Debug("Adding Render layer exclude renderer");
            AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
        }

        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.LightGray;

            SetDesignResolution(Width, Height, SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.SetSize(Width, Height);

            //var deferredRenderer = AddRenderer(new DeferredLightingRenderer(0, LightLayer, RenderablesLayer))
            //    .SetClearColor(Color.DarkGray);
            //deferredRenderer.EnableDebugBufferRender = false;

            var map = Content.LoadTiledMap("Content/pathfinding2-map1.tmx");

            CreateEntity("tiled-map").AddComponent(new TiledMapRenderer(map, "collision"));

            var ff = CreateEntity("flow-field").AddComponent(new FlowFieldOld(map));
            CreateEntity("pathfinder").AddComponent(new PathfinderComponent(map, ff));

            for (int i = 0; i < 1; i++)
            {
                var collider = new CircleCollider(10f)
                {
                    LocalOffset = new Vector2(16, 28)
                };

                CreateEntity("unit-" + i, new Vector2(64 + i % 4 * 40, 64 + i / 4 * 40))
                    .AddComponent(new UnitComponent())
                    .AddComponent(collider);
            }

            var model = Content.Load<Model>("test-model");
            var model3d = new Model3D(model);

            model3d.EnableDefaultLighting();

            //CreateEntity("test-model-entity", new Vector2(128, 128)).AddComponent(model3d);

            /*for (int i = 0; i < Width / 32; i++)
            {
                for (int j = 0; j < Height / 32; j++)
                {
                    if (Random.Chance(0.2f))
                    CreateEntity($"block-{i}-{j}", new Vector2(i * 32, j * 32))
                        .AddComponent(new BlockComponent())
                        .AddComponent(new BoxCollider(0, 0, 32, 32));
                }
                
            }*/

            //Camera.SetRotationDegrees(30);

            var texture = Content.Load<Texture2D>("test-texture");
            //var terrain = CreateEntity("mesh3d-test", new Vector2(0, 1024)).AddComponent(new Mesh3D(32, 32, Color.Red, texture));
        }

        public override void Update()
        {
            if (Input.IsKeyDown(Keys.A))
                Camera.Position -= Vector2.UnitX * 4;
            if (Input.IsKeyDown(Keys.D))
                Camera.Position += Vector2.UnitX * 4;
            if (Input.IsKeyDown(Keys.W))
                Camera.Position -= Vector2.UnitY * 4;
            if (Input.IsKeyDown(Keys.S))
                Camera.Position += Vector2.UnitY * 4;

            if (Input.IsKeyDown(Keys.T))
            {
                Camera.PositionZ3D += 100f;
            }
            if (Input.IsKeyDown(Keys.G))
            {
                Camera.PositionZ3D -= 100f;
            }

            if (Input.IsKeyDown(Keys.Up))
            {
                Camera.ZoomIn(0.05f);
            }
            if (Input.IsKeyDown(Keys.Down))
            {
                Camera.ZoomOut(0.05f);
            }

            if (Input.IsKeyDown(Keys.Left))
            {
                Camera.FarClipPlane3D -= 0.1f;
            }
            if (Input.IsKeyDown(Keys.Right))
            {
                Camera.FarClipPlane3D += 0.1f;
            }

            base.Update();
        }
    }
}