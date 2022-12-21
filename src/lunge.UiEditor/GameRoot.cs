using System;
using lunge.UiEditor.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.ImGuiTools;

namespace lunge.UiEditor;

public class GameRoot : Core
{
    public GameRoot()
    {
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (sender, args) =>
        {
            Console.WriteLine($"{Screen.Width}x{Screen.Height}");
        };
    }

    protected override void Initialize()
    {
        base.Initialize();

        var imGuiManager = new ImGuiManager();
        imGuiManager.ShowSceneGraphWindow = false;
        imGuiManager.ShowDemoWindow = false;
        imGuiManager.ShowCoreWindow = false;
        
        RegisterGlobalManager(imGuiManager);

        Scene = new EditorScene();
    }
}
