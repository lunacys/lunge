using ImGuiNET;
using lunge.UiEditor.Components;
using Nez;
using Nez.ImGuiTools;

namespace lunge.UiEditor.Scenes;

public class EditorScene : Scene
{
    public EditorScene()
    {
        AddRenderer(new DefaultRenderer());
    }

    public override void Initialize()
    {
        Screen.SetSize(1440, 762);
        SetDesignResolution(800, 500, SceneResolutionPolicy.ShowAllPixelPerfect);

        base.Initialize();

        CreateEntity("ui-viewer").AddComponent(new EditorUiViewerComponent());
        CreateEntity("editor-code").AddComponent(new EditorCodeComponent());
    }
}