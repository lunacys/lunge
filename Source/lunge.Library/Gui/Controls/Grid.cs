using System;
using lunge.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Controls
{
    public class Grid : ControlBase, IGraphicsControl
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                InitializeCells();
            }
        }
        public Size2 Size { get; set; }
        public Size2 CellSize { get; set; }
        public float BorderWidth { get; set; }

        public GridCell[,] Cells { get; private set; }

        public Grid(string name, Size2 gridSize, Size2 cellSize, IControl parentControl = null) 
            : base(name, parentControl)
        {
            Size = gridSize;
            CellSize = cellSize;
            
            var width = (int)Math.Floor(gridSize.Width / cellSize.Width);
            var height = (int)Math.Floor(gridSize.Height / cellSize.Height);
            Cells = new GridCell[width, height];

            Position = Vector2.Zero;
            BorderWidth = 1.0f;
        }

        private void InitializeCells()
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    var position = new Vector2(Position.X + i * CellSize.Width, Position.Y + j * CellSize.Height);
                    Cells[i, j] = new GridCell(position, CellSize);
                }
            }
        }

        public GridCell GetCellAt(int x, int y)
        {
            return Cells[x, y];
        }

        public void ForEach(Action<GridCell, int, int> action)
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    action(Cells[i, j], i, j);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cells[i, j].Draw(spriteBatch);
                }
            }

            spriteBatch.DrawRectangle(Position, Size, Color.Black, BorderWidth);
        }
    }
}