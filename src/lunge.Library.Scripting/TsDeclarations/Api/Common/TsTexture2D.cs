using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lunge.Library.Scripting.TsDeclarations.Api.Common;

[TsInterface(typeof(Texture2D), "Texture2D")]
public class TsTexture2D
{
    [TsExclude]
    internal Texture2D OrigTexture2D { get; set; } = null!;

    public int Width => OrigTexture2D.Width;
    public int Height => OrigTexture2D.Height;
    public Rectangle Bounds => OrigTexture2D.Bounds;
    public string Name => OrigTexture2D.Name;

    [TsExclude]
    public static implicit operator Texture2D(TsTexture2D ts)
    {
        return ts.OrigTexture2D;
    }
    [TsExclude]
    public static implicit operator TsTexture2D(Texture2D ts)
    {
        return new TsTexture2D { OrigTexture2D = ts };
    }
}