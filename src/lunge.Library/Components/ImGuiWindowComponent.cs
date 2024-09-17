using System.Numerics;
using ImGuiNET;
using Nez;
using Nez.ImGuiTools;

namespace lunge.Library.Components;

public abstract class ImGuiWindowComponent : Component
{
    public Scene Scene { get; private set; }

    public override void Initialize()
    {
        Scene = Entity.Scene;
    }

    public override void OnEnabled()
    {
        Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(OnDraw);
    }

    public override void OnDisabled()
    {
        Core.GetGlobalManager<ImGuiManager>().UnregisterDrawCommand(OnDraw);
    }

    protected virtual void OnDraw() { }

    protected void ImGuiTooltip(string message, bool sameLine = true)
    {
        if (sameLine)
        {
            ImGui.SameLine();
            ImGui.TextDisabled("(?)");
        }

        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text(message);
            ImGui.EndTooltip();
        }
    }

    protected void ImGuiSectionTitle(string title)
    {
        ImGui.Dummy(new Vector2(0, 10));
        ImGui.TextColored(new Vector4(1, .546f, 0, 1), title);
        ImGui.Separator();
    }
}