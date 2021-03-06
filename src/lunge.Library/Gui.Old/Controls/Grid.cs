﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace lunge.Library.Gui.Old.Controls
{
    [Obsolete("This GUI system is obsolete, please use the new one from the lunge.Library.Gui namespace")]
    public class Grid : ControlBase
    {
        private Vector2 _position;
        public override Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                MoveCells();
            }
        }
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

            InitializeCells();

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

        private void MoveCells()
        {
            ForEach((cell, x, y) =>
            {
                var newPos = new Vector2(Position.X + x * CellSize.Width, Position.Y + y * CellSize.Height);
                cell.Position = newPos;
            });
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