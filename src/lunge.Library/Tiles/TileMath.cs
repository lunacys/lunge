using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.Tiles;

public static class TileMath
{
    public static Point WorldToTileOffset(int mapWidth, int mapHeight, int tileWidth, int tileHeight, Vector2 pos, bool clamp = true)
    {
        var halfW = tileWidth / 2;
        var halfH = tileHeight / 2;

        pos.X -= (mapHeight - 1) * halfW;


        var x = Mathf.FastFloorToInt(((pos.X - halfW) / halfW + ((pos.Y) / halfH)) / 2);
        var y = Mathf.FastFloorToInt(((pos.Y - halfH) / halfH - ((pos.X) / halfW)) / 2) + 1;
        
        if (!clamp)
            return new Point(x, y);

        return new Point(
            Mathf.Clamp(x, 0, mapWidth - 1),
            Mathf.Clamp(y, 0, mapHeight - 1)
        );
    }

    public static Vector2 TileToWorldOffset(int mapHeight, int tileWidth, int tileHeight, Point pos)
    {
        return new Vector2(
            (pos.X - pos.Y) * (tileWidth / 2) + (mapHeight - 1) * tileWidth / 2,
            (pos.X + pos.Y) * (tileHeight / 2)
        );
    }
    
    public static int GetColumn(float mouseX, int firstTileXShiftAtScreen, int columnWidth) 
    {
        return (int)((mouseX - firstTileXShiftAtScreen) / columnWidth);
    }
    
    private static bool TileExists(int x, int y, int width, int height) 
    {
        return x >= 0 & y >= 0 & x < width & y < height; 
    }

    public class TileData
    {
        public int X;
        public int Y;
        public bool IsLeft;
    }

    public static List<TileData> GetTilesInColumn(int columnNo, int width, int height)
    {
        var startTileX = 0;
        var startTileY = 0;
        var xShift = true;
        for (var i = 0; i < columnNo; i++)
        {
            if (TileExists(startTileX + 1, startTileY, width, height))
            {
                startTileX++;
            }
            else
            {
                if (xShift)
                {
                    xShift = false;
                }
                else
                {
                    startTileY++;
                }
            }
        }

        var tilesInColumn = new List<TileData>();
        while (TileExists(startTileX, startTileY, width, height))
        {
            tilesInColumn.Add(new TileData
            {
                X = startTileX, Y = startTileY, IsLeft = xShift
            });
            if (xShift)
            {
                startTileX--;
            }
            else
            {
                startTileY++;
            }

            xShift = !xShift;
        }

        return tilesInColumn;
    }
}