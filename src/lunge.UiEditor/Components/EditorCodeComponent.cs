using System;
using System.IO;
using System.Numerics;
using ImGuiNET;
using Nez;
using Nez.ImGuiTools;

namespace lunge.UiEditor.Components;

public class EditorCodeComponent : Component
{
    private bool _isAutoApplyEnabled;
    private string _codeText = "";
    private string _inputPath = "./code.js";
    
    public EditorCodeComponent()
    {
    }

    public override void OnAddedToEntity()
    {
        var imGuiManager = Core.GetGlobalManager<ImGuiManager>();
        ImGui.SetNextWindowSize(new Vector2(800, 300));
        imGuiManager.RegisterDrawCommand(DrawCode);
    }

    private void LoadCode()
    {
        try
        {
            var text = File.ReadAllText(_inputPath);
            _codeText = text;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void SaveCode()
    {
        try
        {
            File.WriteAllText(_inputPath, _codeText);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void ApplyCode()
    {
        
    }
    
    private unsafe void DrawCode()
    {
        ImGui.Begin("Code Window");
        
        ImGui.InputText("##CodeFilePath", ref _inputPath, 256);
        ImGui.SameLine();
        if (ImGui.Button("Load##LoadCode"))
        {
            LoadCode();
        }
        ImGui.SameLine();
        if (ImGui.Button("Save##SaveCode"))
        {
            SaveCode();
        }
        ImGui.SameLine();
        if (ImGui.Button("Apply Changes"))
        {
            ApplyCode();
        }
        //ImGui.SameLine();
        //ImGui.Checkbox("Auto-apply", ref _isAutoApplyEnabled);

        if (ImGui.InputTextMultiline(
                "##CodeTextbox",
                ref _codeText,
                10000,
                new Vector2(780, 260) ,
                ImGuiInputTextFlags.Multiline | ImGuiInputTextFlags.AllowTabInput,
                data =>
                {
                    Console.WriteLine("Callback");
                    // 0 for success
                    return 0;
                } 
            ))
        {
            
        }
        
        ImGui.End();
    }
}