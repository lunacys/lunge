using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Old.Controls
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public enum ChartType
    {
        Bars,
        Lines,
        Points
    }

    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public class Chart : ControlBase
    {
        public Size2 CellSize { get; set; }

        private float _minValue;
        public float MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                //CellSize = new Size2(CellSize.Width, value == 0 ? value : Size.Height / value);
            }
        }

        private float _maxValue;

        public float MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                CellSize = new Size2(CellSize.Width, value == 0 ? value : Size.Height * 10 / value);
            }
        }

        public bool DrawBackground { get; set; }
        public bool DrawGrid { get; set; }
        public ChartType Type { get; set; }

        public ChartNode LastNode => _nodes.LastOrDefault();

        private List<ChartNode> _nodes = new List<ChartNode>();

        private int _xOffset = 0;

        public Chart(string name, ChartType type = ChartType.Bars) 
            : base(name)
        {
            Type = type;
            
            CellSize = new Size2(8, 8);
        }

        public void Push(float value)
        {
            var node = new ChartNode(LastNode, value, LastNode == null ? 0 : LastNode.XOffset + CellSize.Width);

            if (value < MinValue)
                MinValue = value;
            if (value > MaxValue)
                MaxValue = value;

            _nodes.Add(node);

            if (_nodes.Count > 0 && LastNode.XOffset > Size.Width - _xOffset)
            {
                _xOffset -= (int)CellSize.Width;
                _nodes.Remove(_nodes.First());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (DrawBackground)
            {
                spriteBatch.FillRectangle(Position, Size, Color.AntiqueWhite);
            }
            else
            {
                spriteBatch.DrawLine(new Vector2(Position.X, Position.Y + Size.Height), 
                    new Vector2(Position.X + Size.Width, Position.Y + Size.Height), Color.Black);
            }

            /*if (DrawGrid)
            {
                for (int i = (int)Position.X; i < Size.Width + Position.X; i += (int)CellSize.Width)
                {
                    for (int j = (int)Position.Y; j < Size.Height + Position.Y; j += (int)CellSize.Height)
                    {
                        var rect = new RectangleF(
                            new Point2(i, j),
                            CellSize
                            );
                        spriteBatch.DrawRectangle(rect, Color.Gray * 0.5f);
                    }
                }
            }*/

            if (_nodes.Any())
            {
                foreach (var node in _nodes)
                {
                    var pos = new Vector2(
                        Position.X + node.XOffset + _xOffset,
                        Position.Y - MathUtils.NormalizeInRange(Size.Height, node.Value, MinValue, MaxValue) + Size.Height
                    );

                    if (pos.X > Position.X)
                    {
                        if (Type == ChartType.Points)
                        {
                            spriteBatch.DrawPoint(pos, Color.Black, 4f);
                        }
                        else if (Type == ChartType.Lines)
                        {
                            spriteBatch.DrawPoint(pos, Color.Black, 4f);

                            if (node.Next != null)
                            {
                                var nextPos = new Vector2(
                                    Position.X + node.Next.XOffset + _xOffset,
                                    Position.Y - MathUtils.NormalizeInRange(Size.Height, node.Next.Value, MinValue, MaxValue) + Size.Height
                                );

                                spriteBatch.DrawPoint(nextPos, Color.Black, 4f);
                                spriteBatch.DrawLine(pos, nextPos, Color.Black, 1f);
                            }
                        }
                        else if (Type == ChartType.Bars)
                        {

                        }
                    }

                    /*if (pos.X > Position.X)
                    {

                    }*/
                }
            }

            base.Draw(spriteBatch);
        }
    }
}