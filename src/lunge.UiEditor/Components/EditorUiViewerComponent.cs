using Nez;

namespace lunge.UiEditor.Components;

public class EditorUiViewerComponent : Component
{
    private UICanvas _canvas;

    public override void OnAddedToEntity()
    {
        _canvas = Entity.AddComponent(new UICanvas());
        _canvas.IsFullScreen = true;

        _canvas.DebugRenderEnabled = true;

        base.OnAddedToEntity();
    }
}