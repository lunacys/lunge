using System;
using System.Collections.Generic;
using ImGuiNET;
using lunge.Library.Debugging;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.ImGuiTools;
using Nez.Splines;

namespace Playground.Components;

public class CatmullRomTests : RenderableComponent
{
    private System.Numerics.Vector2 
        _value1 = new (64, 64),
        _value2 = new (100, 64), 
        _value3 = new (128, 100),
        _value4 = new (150, 150),
        _start = new (64, 64);

    private float _amount = 0.5f;
    private int _maxSteps = 20;
    private Vector2 _catmullRom;

    public override RectangleF Bounds => new RectangleF(0, 0, 2000, 2000);

    private BezierSpline _spline = new BezierSpline();

    private TimeSpan _lastTime1, _lastTime2;

    public List<Vector2> CurvePoints { get; private set; } = new List<Vector2>();

    private LineRenderer _lineRenderer;

    public override void OnAddedToEntity()
    {
        Core.GetGlobalManager<ImGuiManager>()?.RegisterDrawCommand(Draw);
        //_lineRenderer = Entity.GetComponent<LineRenderer>();
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        /*var v = Vector2.CatmullRom(_value1, _value2, _value3, _value4, _amount);

        DrawLine(batcher, _start, v, Color.Red, 2f);*/

        /*var knots = new List<Vector2>
        {
            _value1, _value2, _value3, _value4
        };
        _catmullRom = Vector2.CatmullRom(_value1, _value2, _value3, _value4, _amount);

        // List<Vector2> curvePoints = null;

        _lastTime1 = Debug.TimeAction(() =>
        {
            var controls = MathUtils.GenerateControlPoints(knots, _amount);
            CurvePoints = MathUtils.GenerateCurvePoints(knots, controls, _maxSteps);
            _lineRenderer?.ClearPoints();
            _lineRenderer?.SetPoints(CurvePoints.ToArray());
        });
        

        for (var i = 0; i < CurvePoints.Count - 1; i++)
        {
            var curvePoint = CurvePoints[i];
            var next = CurvePoints[i + 1];
            

            //DrawLine(batcher, curvePoint, next, Color.Black);
            batcher.DrawArrow(curvePoint, next, Color.Black, 6, 2f);
            batcher.DrawPixel(curvePoint.X, curvePoint.Y, Color.Red, 8);
        }

        foreach (var knot in knots)
        {
            batcher.DrawCircle(knot, 8f, Color.Yellow, 1f, 32);
        }

        var a = Vector2.Zero;
        var m = Matrix2D.CreateRotation(3.14f);
        Vector2.Transform(a, m);

        batcher.DrawCircle(_catmullRom, 6f, Color.Blue, 2f, 32);

        _lastTime2 = Debug.TimeAction(() =>
        {
            var points = Bezier.GetOptimizedDrawingPoints(_value1, _value2, _value3, _value4, _amount);
        });
        
        /*for (var i = 0; i < points.Count - 1; i++)
        {
            var curvePoint = points[i];
            var next = points[i + 1];


            DrawLine(batcher, curvePoint, next, Color.Green);
            batcher.DrawPixel(curvePoint.X, curvePoint.Y, Color.Red, 8);
        }*/
    }

    private void Draw()
    {
        ImGui.Begin("Catmull Rom Settings");

        ImGui.SliderFloat2("Value1", ref _value1, 0f, 1000f);
        ImGui.SliderFloat2("Value2", ref _value2, 0f, 1000f);
        ImGui.SliderFloat2("Value3", ref _value3, 0f, 1000f);
        ImGui.SliderFloat2("Value4", ref _value4, 0f, 1000f);
        // ImGui.SliderFloat2("Start", ref _start, 0f, 1000f);
        ImGui.SliderFloat("Amount", ref _amount, 0f, 10f);
        ImGui.SliderInt("Max Steps", ref _maxSteps, 4, 200);
        
        ImGui.Text($"Catmull: {_catmullRom}");
        ImGui.Text($"LastTime1: {_lastTime1.TotalMilliseconds} ms");
        ImGui.Text($"LastTime2: {_lastTime2.TotalMilliseconds} ms");

        ImGui.End();
    }

    private void DrawLine(Batcher batcher, Vector2 start, Vector2 end, Color color, float thickness = 2f)
    {
        var delta = end - start;
        var angle = (float)Math.Atan2(delta.Y, delta.X);
        batcher.Draw(Graphics.Instance.PixelTexture, start + Entity.Transform.Position + LocalOffset,
            Graphics.Instance.PixelTexture.SourceRect, color, angle, new Vector2(0, 0.5f),
            new Vector2(delta.Length(), thickness), SpriteEffects.None, LayerDepth);
    }
}