using lunge.Library.Scripting;
using Nez;
using Nez.ImGuiTools;

namespace Playground2.Components.ModTesting;

public class ModHandlerComponent : Component, IUpdatable
{
    private ModHandler _modHandler;

    public ModHandlerComponent(string modFolderPath)
    {
        _modHandler = new ModHandler(modFolderPath, Entity.Scene.Content);

        _modHandler.Execute();
    }

    public override void OnAddedToEntity()
    {
        Core.GetGlobalManager<ImGuiManager>().RegisterDrawCommand(ImGuiDraw);
    }

    public void Update()
    {
        
    }

    private void ImGuiDraw()
    {/*
        ImGui.Begin("Debugger");

        var i = 1;

        foreach (var step in _modHandler.DebugInformation)
        {
            var header = $"Step #{i}: {{{step.Location.Start}}}:{{{step.Location.End}}}";

            if (step.Location.Source != null)
                header += $" ({step.Location.Source})";
            if (step.BreakPoint != null)
                header += " BREAKPOINT";
            if (step.ReturnValue != null)
            {
                header += $". Return Value: {step.ReturnValue}";
            }

            if (ImGui.CollapsingHeader(header))
            {
                ImGui.Indent();

                ImGui.Text($"Current Memory Usage: {step.CurrentMemoryUsage}");

                if (step.BreakPoint != null && ImGui.CollapsingHeader($"{step.BreakPoint.Location}"))
                {
                    ImGui.Text($"Condition: {step.BreakPoint.Condition}");
                }

                if (ImGui.CollapsingHeader($"Call Stack ##{i}"))
                {
                    ImGui.Indent();

                    foreach (var callFrame in step.CallStack)
                    {
                        ImGui.Text($"Location: {callFrame.Location}\n" +
                                   $"Function Location: {callFrame.FunctionLocation}\n" +
                                   $"Return Value: {callFrame.ReturnValue}\n" +
                                   $"Function Name: {callFrame.FunctionName}\n" +
                                   $"Scope Chain: {callFrame.ScopeChain}\n" +
                                   $"This: {callFrame.This}");
                    }

                    ImGui.Unindent();
                }

                if (ImGui.CollapsingHeader($"Current Call Frame ##{i}"))
                {
                    ImGui.Indent();

                    var callFrame = step.CurrentCallFrame;

                    ImGui.Text($"Location: {callFrame.Location}\n" +
                               $"Function Location: {callFrame.FunctionLocation}\n" +
                               $"Return Value: {callFrame.ReturnValue}\n" +
                               $"Function Name: {callFrame.FunctionName}\n" +
                               $"Scope Chain: {callFrame.ScopeChain}\n" +
                               $"This: {callFrame.This}");

                    ImGui.Unindent();
                }

                if (step.CurrentNode != null && ImGui.CollapsingHeader($"Current Node ##{i}"))
                {
                    ImGui.Indent();

                    var node = step.CurrentNode;

                    var txt = $"Location: {node.Location}\n" +
                              $"Data: {node.Data}\n" +
                              $"Range: {node.Range}\n" +
                              $"Type: {node.Type}\n" +
                              $"Child Nodes:";

                    foreach (var childNode in node.ChildNodes)
                    {
                        txt += $"{childNode}\n";
                    }

                    ImGui.Text(txt);

                    ImGui.Unindent();
                }

                if (ImGui.CollapsingHeader($"Current Scope Chain ##{i}"))
                {
                    ImGui.Indent();
                    
                    foreach (var scope in step.CurrentScopeChain)
                    {
                        var txt =
                            $"Is Top Level: {scope.IsTopLevel}\n" +
                            $"Binding Object: {scope.BindingObject}\n" +
                            $"Scope Type: {scope.ScopeType}\n" +
                            $"Binding Names: ";

                        /*foreach (var bindingName in scope.BindingNames)
                        {
                            var val = _modHandler.Get(bindingName);
                            txt += " > " + bindingName + " " + val + "\n";
                        }#1#

                        /*if (scope.BindingObject != null)
                        {
                            foreach (var props in scope.BindingObject.GetOwnProperties())
                            {
                                txt += $" > {props.Key}: {props.Value}\n";
                            }
                        }#1#

                        ImGui.Text(txt);
                    }

                    ImGui.Unindent();
                }

                ImGui.Unindent();
            }

            i++;
        }

        ImGui.End();*/
    }
}