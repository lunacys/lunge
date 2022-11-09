using ImGuiNET;
using lunge.Library;
using lunge.Library.AI.Steering;
using lunge.Library.AI.Steering.Behaviors;
using lunge.Library.AI.Steering.Pathing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.ImGuiTools;
using Nez.Sprites;
using Random = Nez.Random;

namespace Playground2.Components.SteeringBehaviors
{
    public class SteeringBehaviorsComponent : RenderableComponent, IUpdatable
    {
        public override float Width => 1360;
        public override float Height => 768;

        private Texture2D _defaultTexture;
        private Texture2D _obstacleTexture;

        private PathComponent _pathBuilder;

        [Inspectable]
        public PathFollowingMode PathFollowingMode = PathFollowingMode.OneWay;

        private int _imGuiMode;

        public SteeringBehaviorsComponent(PathComponent pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public override void Initialize()
        {
            base.Initialize();

            Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(UiLayout);

            _defaultTexture = Core.Content.Load<Texture2D>("steering/YellowArrow");
            _obstacleTexture = Core.Content.Load<Texture2D>("steering/GreenSquare");

            Debug.DrawTextFromBottom = true;
        }

        void IUpdatable.Update()
        {
            if (Input.LeftMouseButtonPressed)
            {
                AddObstacle(Input.MousePosition);
            }
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            UiLayout();
        }

        public void UiLayout()
        {
            ImGui.Begin("Steering Behaviors");
            ImGui.TextWrapped("Choose a steering behavior to add:");
            ImGui.TextWrapped("Seek: the most simple steering behavior, only seeking the target");
            if (ImGui.Button("Seek"))
            {
                AddSeek();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Flee: the counter of Seek - flee away from the target");
            if (ImGui.Button("Flee"))
            {
                AddFlee();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Arrival: a seeker with arrival radius which the more slows down the nearer to the target");
            if (ImGui.Button("Arrival"))
            {
                AddArrival();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Wander: just wanders around in random manner");
            if (ImGui.Button("Wander"))
            {
                AddWander();
            }
            ImGui.SameLine();
            if (ImGui.Button("Wander x10"))
            {
                AddWander(10);
            }
            ImGui.SameLine();
            if (ImGui.Button("Wander x20"))
            {
                AddWander(20);
            }
            ImGui.SameLine();
            if (ImGui.Button("Wander x100"))
            {
                AddWander(100);
            }
            ImGui.Separator();
            ImGui.TextWrapped("Pursuit: hunting the target down with kind of prediction");
            if (ImGui.Button("Pursuit"))
            {
                AddPursuit();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Evade: the opposite of pursuit - just like Flee - getting away from the target with some kind of prediction");
            if (ImGui.Button("Evade"))
            {
                AddEvade();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Collision Avoidance: trying to avoid obstacles");
            if (ImGui.Button("Collision Avoidance"))
            {
                AddCollisionAvoidance();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Path Following: just as simple as that, following the path with 'natural' movement");
            if (ImGui.Button("Path Following"))
            {
                AddPathFollowing();
            }
            ImGui.Text("Circular motion");
            ImGui.SameLine();
            int newMode = _imGuiMode;
            ImGui.ListBox("Path Following Mode", ref newMode, new[] { "One Way", "Circular", "Patrol" }, 3);
            ImGui.Separator();

            ImGui.TextWrapped("Leader Following: a composition of other steering forces, all arranged to make a group of characters follow a specific character");
            if (ImGui.Button("Leader Following"))
            {
                AddLeaderFollowing();
            }
            ImGui.Separator();
            ImGui.TextWrapped("Queue: process of standing in line, forming a row of characters that are patiently waiting to arrive somewhere");
            if (ImGui.Button("Queue"))
            {
                AddQueue();
            }

            ImGui.End();

            if (newMode != _imGuiMode)
            {
                _imGuiMode = newMode;
                PathFollowingMode = (PathFollowingMode)_imGuiMode;

                Debug.Log("CHANGE!!!");
            }
        }

        private void AddSeek()
        {
            Debug.Log("Adding Seek");

            var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddBehavior(new Seek())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);
        }

        private void AddFlee()
        {
            Debug.Log("Adding Flee");

            var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddBehavior(new Flee())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);
        }

        private void AddArrival()
        {
            Debug.Log("Adding Arrival");

            var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddBehavior(new Arrival(128f))
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);
        }

        private void AddWander(int count = 1)
        {
            Debug.Log("Adding Wander x" + count);

            for (int i = 0; i < count; i++)
            {
                var entity = new SteeringBuilder(new Vector2(500, 400) + Random.RNG.NextVector2(-100, 100))
                    .AddBehavior(new Wander(6, 8, 0, 0.5f))
                    .UseDefaultRenderer(_defaultTexture)
                    .AddCollider(12f)
                    .Build();

                Core.Scene.AddEntity(entity);
            }

        }

        private void AddPursuit()
        {
            Debug.Log("Adding Pursuit");

            var seek = new SteeringBuilder(new Vector2(128, 128))
                .AddBehavior(new Seek())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            var entity = new SteeringBuilder(new Vector2(512, 512), seek)
                .SetPhysicalParams(new SteeringPhysicalParams { Mass = 5.0f, MaxForce = 2.0f, MaxVelocity = 2.6f })
                .AddBehavior(new Pursuit())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(seek);
            Core.Scene.AddEntity(entity);
        }

        private void AddEvade()
        {
            Debug.Log("Adding Evade");

            var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddBehavior(new Evade())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);
        }

        private void AddObstacle(Vector2 position)
        {
            Core.Scene.CreateEntity("obstacle-" + position, position)
                .AddComponent(new SpriteRenderer(_obstacleTexture))
                .AddComponent(new BoxCollider(-32, -32, 64, 64))
                .PhysicsLayer = 2;
        }

        private void AddCollisionAvoidance()
        {
            Debug.Log("Adding Collision Avoidance");

            var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddCollider(12f)
                .AddBehavior(new Seek())
                .AddBehavior(new CollisionAvoidance(300f, 800f))
                .UseDefaultRenderer(_defaultTexture)
                .Build();

            Core.Scene.AddEntity(entity);
        }

        private void AddPathFollowing()
        {
            Debug.Log("Adding Path Following");

            var entity = new SteeringBuilder(new Vector2(512, 512), _pathBuilder)
                .AddBehavior(new PathFollowing(new Seek(), PathFollowingMode))
                .AddBehavior(new CollisionAvoidance(100f, 800f))
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);
        }
        private void AddLeaderFollowing()
        {
            Debug.Log("Adding Leader Following");

            /*var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddBehavior(new LeaderFollowing())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);*/
        }
        private void AddQueue()
        {
            Debug.Log("Adding Queue");

            /*var entity = new SteeringBuilder(new Vector2(512, 512))
                .AddBehavior(new Queue())
                .UseDefaultRenderer(_defaultTexture)
                .AddCollider(12f)
                .Build();

            Core.Scene.AddEntity(entity);*/
        }

    }
}