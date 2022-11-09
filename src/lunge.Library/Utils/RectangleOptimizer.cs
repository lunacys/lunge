using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace lunge.Library.Utils;

public static class RectangleOptimizer
{
    /// <summary>
    /// Returns a list of rectangles in tile space, where any non-null tile is combined into bounding regions
    /// </summary>
    public static List<Rectangle> GetCollisionRectangles(bool[,] map, int tileW, int tileH)
    {
        var height = map.GetLength(0);
        var width = map.GetLength(1);

        var checkedIndexes = new bool?[width * height];
        var rectangles = new List<Rectangle>();
        var startCol = -1;
        var index = -1;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                index = y * width + x;
                var tile = map[y, x];

                if (tile && (checkedIndexes[index] == false || checkedIndexes[index] == null))
                {
                    if (startCol < 0)
                        startCol = x;

                    checkedIndexes[index] = true;
                }
                else if (!tile  || checkedIndexes[index] == true)
                {
                    if (startCol >= 0)
                    {
                        rectangles.Add(FindBoundsRect(map, startCol, x, y, tileW, tileH, checkedIndexes));
                        startCol = -1;
                    }
                }
            } // end for x

            if (startCol >= 0)
            {
                rectangles.Add(FindBoundsRect(map, startCol, width, y, tileW, tileH, checkedIndexes));
                startCol = -1;
            }
        }

        return rectangles;
    }

    /// <summary>
    /// Finds the largest bounding rect around tiles between startX and endX, starting at startY and going
    /// down as far as possible
    /// </summary>
    public static Rectangle FindBoundsRect(bool[,] map, int startX, int endX, int startY, int tileW, int tileH, bool?[] checkedIndexes)
    {
        var index = -1;
        var height = map.GetLength(0);
        var width = map.GetLength(1);

        for (var y = startY + 1; y < height; y++)
        {
            for (var x = startX; x < endX; x++)
            {
                index = y * width + x;
                var tile = map[y, x];

                if (!tile || checkedIndexes[index] == true)
                {
                    // Set everything we've visited so far in this row to false again because it won't be included in the rectangle and should be checked again
                    for (var _x = startX; _x < x; _x++)
                    {
                        index = y * width + _x;
                        checkedIndexes[index] = false;
                    }

                    return new Rectangle(startX * tileW, startY * tileH,
                        (endX - startX) * tileW, (y - startY) * tileH);
                }

                checkedIndexes[index] = true;
            }
        }

        return new Rectangle(startX * tileH, startY * tileH,
            (endX - startX) * tileW, (height - startY) * tileH);
    }
}