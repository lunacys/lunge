using Microsoft.Xna.Framework;
using Nez.BitmapFonts;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsInterface(typeof(BitmapFont), "BitmapFont")]
public class TsBitmapFont
{
    [TsExclude] internal BitmapFont OrigBitmapFont { get; set; } = null!;

    public Vector2 MeasureString(string text) => OrigBitmapFont.MeasureString(text);
}