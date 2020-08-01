using System.Collections.Generic;
using ImGuiNET;
using lunge.Library.Graphics;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;

namespace lunge.Library.Debugging
{
    public class ImGuiDebug
    {
        private Dictionary<string, IDebugAttachable> _attachables = new Dictionary<string, IDebugAttachable>();
        
        public void Register(string label, IDebugAttachable debugAttachable)
        {
            _attachables[label] = debugAttachable;
        }

        public void Visualize(ImGuiRenderer renderer, GameTime gameTime)
        {
            renderer.BeforeLayout(gameTime);

            ImGui.ShowDemoWindow();
            ImGui.Begin("Visuals");

            foreach (var kv in _attachables)
            {
                if (ImGui.TreeNode(kv.Key))
                {
                    kv.Value.Visualize(renderer);
                    foreach (var kv2 in kv.Value.Attach().Map)
                    {
                        ImGui.Text($"{kv2.Key}: {kv2.Value}");
                    }
                }
            }


            ImGui.End();

            renderer.AfterLayout();
        }
    }
}