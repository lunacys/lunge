using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Playground2.Components.TestsTiledPathfinding;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Playground2.Scenes
{
    public class TestScene : Scene
    {
        public const int ScreenSpaceRenderLayer = 999;
        private TiledMapRenderer _mapEntity;
        private GraphicsDevice _graphicsDevice;

        public TestScene(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;

            AddRenderer(new ScreenSpaceRenderer(100, ScreenSpaceRenderLayer));
            //AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
        }

        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.Black;
            //SetDesignResolution(640, 368, SceneResolutionPolicy.ShowAllPixelPerfect);
            SetDesignResolution(1920 / 2, 1080 / 2, SceneResolutionPolicy.ShowAllPixelPerfect);

            Screen.SetSize(1920, 1080);

            var map = Content.LoadTiledMap("Content/destructable-map.tmx");
            //var map2 = Content.LoadTiledMap("Content/test-isometric-3.tmx");

            var tiledEntity = CreateEntity("tiled-map").AddComponent(new TiledMapRenderer(map));
            //_mapEntity = tiledEntity.AddComponent(new TiledMapRenderer(map2));
            //_mapEntity.Transform.Position = new Vector2(512, 0);
            CreateEntity("pathfinder").AddComponent(new Pathfinder(map));
            //CreateEntity("pathfindervertex").AddComponent(new PathfinderVertex(map));

            //var map3 = Content.Load<TiledMap>("test-isometric-2");
            //var tiled2 = CreateEntity("tiled-2").AddComponent(new TiledMapRenderer2(_graphicsDevice, map3));
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

            base.Update();
        }


    }
}