using lunge.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnexpectedJourney
{
    public class TileSet
    {
        public int TileWidth { get; }
        public int TileHeight { get; }
        public int TileColumns { get; }
        public int TileRows { get; }

        public Texture2D TileSetImage { get; }

        private readonly Texture2D[,] _tiles;

        public Texture2D this[int x, int y] => GetTile(x, y);

        public Texture2D this[Point position] => GetTile(position.X, position.Y);

        public TileSet(Texture2D tileSetImage, int tileWidth, int tileHeight)
        {
            TileSetImage = tileSetImage;

            TileWidth = tileWidth;
            TileHeight = tileHeight;

            TileColumns = tileSetImage.Width / tileWidth;
            TileRows = tileSetImage.Height / tileHeight;

            _tiles = new Texture2D[TileColumns, TileRows];
        }

        public Texture2D GetTile(int x, int y)
        {
            if (_tiles[x, y] == null)
                _tiles[x, y] = TileSetImage.Crop(x * TileWidth, y * TileHeight, TileWidth, TileHeight);

            return _tiles[x, y];
        }

        /// <summary>
        /// Clears all the loaded tiles. Use this method if you want to reinitialize tile set
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < TileColumns; i++)
            {
                for (int j = 0; j < TileRows; j++)
                {
                    _tiles[i, j]?.Dispose();
                    _tiles[i, j] = null;
                }
            }
        }

        public Texture2D GetTile(Point position)
        {
            return GetTile(position.X, position.Y);
        }
    }
}