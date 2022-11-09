using Nez;
using Nez.UI;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsInterface(typeof(UICanvas), "UICanvas")]
public class TsUiCanvas
{
    internal UICanvas OrigUiCanvas { get; set; } = null!;

    public float Width => OrigUiCanvas.Width;
    public float Height => OrigUiCanvas.Height;
    public Stage Stage => OrigUiCanvas.Stage;

    public bool IsFullScreen
    {
        get => Stage.IsFullScreen;
        set => Stage.IsFullScreen = value;
    }

    public Dialog ShowDialog(string title, string messageText, string closeButtonText) =>
        OrigUiCanvas.ShowDialog(title, messageText, closeButtonText);
}