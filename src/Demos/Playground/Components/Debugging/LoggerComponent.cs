using System;
using System.Linq;
using ImGuiNET;
using LiteLog.Logging.Loggers;
using Nez;
using Nez.ImGuiTools;

namespace Playground.Components.Debugging;

public class LoggerComponent : Component
{
    private bool _needToScroll = false;

    private string[] _logs = Array.Empty<string>();

    public override void OnAddedToEntity()
    {
        Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(Draw);
        MemoryLogger.LogAdded += (sender, args) =>
        {
            _needToScroll = true;
            _logs = MemoryLogger.Messages.ToArray();
        };
    }

    private void Draw()
    {
        ImGui.Begin("Logs");

        for (var i = 0; i < _logs.Length; i++)
        {
            ImGui.Text(_logs[i]);
        }

        if (_needToScroll)
        {
            ImGui.SetScrollHereY(1f);
            _needToScroll = false;
        }

        ImGui.End();
    }
}