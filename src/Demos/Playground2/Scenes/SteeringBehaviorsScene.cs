using lunge.Library.AI.Steering;
using lunge.Library.AI.Steering.Pathing;
using lunge.Library.Debugging.Profiling;
using Microsoft.Xna.Framework;
using Nez;
using Nez.ImGuiTools;
using Playground2.Components;
using Playground2.Components.SteeringBehaviors;

namespace Playground2.Scenes
{
    public class SteeringBehaviorsScene : SceneTimed<SteeringBehaviorsScene>
    {
        public const int ScreenSpaceRenderLayer = 999;

        private ImGuiManager _imGuiManager;

        public SteeringBehaviorsScene()
        {
            AddRenderer(new ScreenSpaceRenderer(100, ScreenSpaceRenderLayer));
            AddRenderer(new RenderLayerExcludeRenderer(0, ScreenSpaceRenderLayer));
        }

        public override void Initialize()
        {
            base.Initialize();

            ClearColor = Color.CornflowerBlue;

            SetDesignResolution(1360, 768, SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.SetSize(1920, 1080);

            CreateEntity("mouse-entity").AddComponent(new MouseEntityComponent());
            var path = CreateEntity("path-builder").AddComponent(new PathComponent(new Path()));
            CreateEntity("sb-base").AddComponent(new SteeringBehaviorsComponent(path));

            CreateEntity("profiler").AddComponent(new ProfilerComponent());
        }
    }
}