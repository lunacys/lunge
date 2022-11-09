using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Debugging;

public static class DebugRendering
{
    public static void DebugDraw(this Vector2 vector, Batcher batcher, Vector2 position, Color color, float thickness = 1f)
    {
        DrawArrow(batcher, position, vector, color, thickness);
    }

    public static void DebugDraw(this Vector2 vector, Vector2 position, Color color, float thickness = 1f) =>
        DebugDraw(vector, Nez.Graphics.Instance.Batcher, position, color, thickness);

    public static void DrawVector(this Batcher batcher, Vector2 position, Vector2 vector, Color color, float thickness = 1f) =>
        DebugDraw(vector, batcher, position, color, thickness);

    public static void DrawArrow(this Batcher batcher, Vector2 start, Vector2 end, Color color, float arrowLength = 6f, float thickness = 1f)
    {
        var angle = Mathf.AngleBetweenVectors(start, end);

        var mat = Matrix2D.CreateRotation(angle);
        // var firstQ =Quaternion.CreateFromYawPitchRoll(0, 0, angle);
        var offset1 = Vector2.Transform(new Vector2(-arrowLength, -arrowLength), mat);
        var offset2 = Vector2.Transform(new Vector2(-arrowLength, arrowLength), mat);

        batcher.DrawLine(start, end, color, thickness);
        batcher.DrawLine(end, end + offset1, color, thickness);
        batcher.DrawLine(end, end + offset2, color, thickness);
    }
}