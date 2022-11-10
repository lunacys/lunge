using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsInterface(typeof(Batcher), "Batcher")]
public class TsBatcher
{
    [TsExclude] internal Batcher OrigBatcher { get; set; } = null!;

    // public void Draw(TsSprite sprite, Vector2 position, Color color, float rotation, Vector2 origin, float scale,
    //     SpriteEffects spriteEffects, float layerDepth) =>
    //     OrigBatcher.Draw(sprite.OrigSprite, position, color, rotation, origin, scale, spriteEffects, layerDepth);
    //
    //
    // public void Draw(TsSprite sprite, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale,
    //     SpriteEffects spriteEffects, float layerDepth) =>
    //     OrigBatcher.Draw(sprite.OrigSprite, position, color, rotation, origin, scale, spriteEffects, layerDepth);
    //
    // public void DrawString(IFont font, string text, Vector2 position, Color color) =>
    //     OrigBatcher.DrawString(font, text, position, color);
    //
    // public void DrawString(IFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth) =>
    //     OrigBatcher.DrawString(font, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
    //
    // public void DrawString(IFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth) =>
    //     OrigBatcher.DrawString(font, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);

    public static implicit operator Batcher(TsBatcher batcher) => batcher.OrigBatcher;
    public static implicit operator TsBatcher(Batcher batcher) => new TsBatcher { OrigBatcher = batcher };
}