using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI.Steering.Old.Pathing
{
    public class PathComponent : Component, ISteeringTarget, IUpdatable
    {
        public Vector2 Position
        {
            get => Path.GetTargetNode()?.Target ?? Vector2.Zero;
            set => Path.AddNode(value);
        }

        public bool IsActual => Path.NodeCount > 0;

        public Path Path { get; set; }

        public PathComponent(Path path)
        {
            Path = path;
        }

        public void Update()
        {
            // TODO: Remove this
            if (Nez.Input.RightMouseButtonPressed && Nez.Input.MousePosition.X >= 0 && Nez.Input.MousePosition.Y >= 0)
                Path.AddNode(Nez.Input.MousePosition);
        }

        public override void DebugRender(Batcher batcher)
        {
            if (Path.NodeCount > 1)
            {
                for (int i = 0; i < Path.NodeCount - 1; i++)
                {
                    var cur = Path[i];
                    var next = Path[i + 1];

                    batcher.DrawLine(cur.Target, next.Target, Color.Green);

                    cur.DebugRender(batcher);
                }
            }

            if (Path.NodeCount >= 1)
                Path[Path.NodeCount - 1].DebugRender(batcher);
        }
    }
}