using System;
using ImGuiNET;
using lunge.Library.AI.Pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.ImGuiTools;
using Nez.ImGuiTools.ObjectInspectors;

namespace Playground2.Components.Pathfinding2
{
    public class UnitComponent : RenderableComponent, IUpdatable
    {
        private Texture2D _texture;

        public override float Width => 32;
        public override float Height => 32;

        public bool IsSelected { get; set; } = false;

        private Collider _collider;

        private Vector2 _velocity = Vector2.Zero;


        private Vector2? _target;
        private FlowFieldOld _flowField;
        public Func<Vector2, Point> WorldToTileFunc { get; set; }

        private Point _currTile;

        public UnitComponent()
        {
        }

        private int _combo = 1;
        private string[] _items = new[] { "Item #1", "Item #2", "Item #3" };

        [InspectorDelegate]
        public void TestMethod()
        {
            ImGui.Text("Text!!!!!!!!");
            ImGui.Combo("Combo!", ref _combo, _items, 3);
        }

        public override void Initialize()
        {
            base.Initialize();

            _texture = Core.Content.Load<Texture2D>("test-character");
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            if (IsSelected)
            {
                batcher.DrawCircle(Entity.Position + new Vector2(16, 28), 10f, Color.Red);

                if (_target != null)
                    batcher.DrawCircle(_target.Value, 8, Color.Blue);
            }

            batcher.Draw(_texture, Entity.Position);

            batcher.DrawHollowRect(_currTile.X * 32, _currTile.Y * 32, 32, 32, Color.Yellow);
        }

        public void MoveTo(Vector2 target)
        {
            Debug.Log("Moving unit {0} to {1}", Entity.Name, target);

            _target = target;

        }

        public override void OnAddedToEntity()
        {
            Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(ImGuiDraw);
        }

        private void ImGuiDraw()
        {
            ImGui.Begin("My ImGUI Window");

            ImGui.Text("Text!!!!!!!!!!!!!!!!!!!");

            ImGui.End();
        }

        public void MoveAlongPath(FlowFieldOld flowField)
        {
            _flowField = flowField;
        }

        public void Update()
        {
            /*if (_target.HasValue)
            {
                _velocity = _target.Value - Entity.Position;
                _velocity.Normalize();

                if (Vector2.Distance(Entity.Position, _target.Value) < 6f)
                {
                    _target = null;
                }
            }
            else
            {
                _velocity = Vector2.Zero;
            }*/
            if (_flowField != null)
            {
                _currTile = WorldToTileFunc(Entity.Position + new Vector2(16, 28));
                if (_flowField._flowFieldNodes.ContainsKey(_currTile))
                {
                    var velo = _flowField._flowFieldNodes[_currTile].Direction;
                    if (velo != Vector2.Zero)
                        velo.Normalize();
                    _velocity = velo * 20;
                }
            }

            Entity.Position += _velocity;


            Vector2 motion = Vector2.Zero;
            //CollisionResult result;

            if (_collider == null)
                _collider = Entity.GetComponent<Collider>();
            if (_collider != null && _collider.CollidesWithAny(ref motion, out _))
            {
                // Debug.Log("collision result: {0}", result);
            }

            Entity.Position += motion;

            // Entity.Position = Vector2.Clamp(Entity.Position, Vector2.Zero, new Vector2(1920 / 2, 1080 / 2));
        }
    }
}