using lunge.Library.Scripting.TsDeclarations.Api.Common;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Textures;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsClass(typeof(Sprite), "Sprite")]
public class TsSprite
{
    [TsExclude] internal Sprite OrigSprite { get; set; } = null!;

    public TsTexture2D Texture2D
    {
        get => OrigSprite.Texture2D;
        set => OrigSprite.Texture2D = value;
    }

    public TsSprite(TsTexture2D texture)
    {
        OrigSprite = new Sprite(texture);
    }

    public Rectangle SourceRect => OrigSprite.SourceRect;

    public RectangleF Uvs => OrigSprite.Uvs;

    public Vector2 Center => OrigSprite.Center;

    public Vector2 Origin
    {
        get => OrigSprite.Origin;
        set => OrigSprite.Origin = value;
    }
}