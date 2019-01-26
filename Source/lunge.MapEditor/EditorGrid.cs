namespace lunge.MapEditor
{
    public class EditorGrid
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }

        public EditorGrid(int gridWidth, int gridHeight, int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            GridWidth = gridWidth;
            GridHeight = gridHeight;
        }

        public void SetTile(int x, int y, int tileId)
        {
            
        }
    }
}