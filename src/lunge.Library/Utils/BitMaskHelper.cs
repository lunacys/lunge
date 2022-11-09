using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Utils;

public static class BitMaskHelper
{
    private static Point[] _dirs = new[]
    {
        new Point(0, -1), // top
        new Point(1,  0), // right
        new Point(0,  1), // bottom
        new Point(-1,  0),// left
    };

    private static Point[] _dirsDiag = new[]
    {
        new Point(1, -1), // 1, top-right
        new Point(1, 1), // 2, bottom-right
        new Point(-1, 1), // 4, bottom-left
        new Point(-1, -1), // 8, top-left

        new Point(0, -1), // 16, top
        new Point(1, 0), // 32, right
        new Point(0, 1), // 64, bottom
        new Point(-1, 0) // 128, left
    };

    public static int[,] CalculateBitMaskForBitMap(bool[,] bitMap, bool includeDiagonals = true)
    {
        var h = bitMap.GetLength(0);
        var w = bitMap.GetLength(1);

        var result = new int[h, w];

        var dirsToUse = includeDiagonals ? _dirsDiag : _dirs;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                var bit = bitMap[y, x];
                int newByte = 0;

                if (bit)
                {
                    for (var i = 0; i < dirsToUse.Length; i++)
                    {
                        var dir = dirsToUse[i];
                        var p = new Point(x + dir.X, y + dir.Y);

                        if (IsNotOutOfBounds(p, w, h) && bitMap[p.Y, p.X])
                            newByte |= (int)Math.Pow(2, i);
                    }
                }

                result[y, x] = newByte;
            }
        }

        return result;
    }

    public static bool[,] CreateBitMapFrom<T>(T[,] world, T includeVal) where T : struct
    {
        var h = world.GetLength(0);
        var w = world.GetLength(1);

        var result = new bool[h, w];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                result[y, x] = world[y, x].Equals(includeVal);
            }
        }

        return result;
    }

    public static bool IsNotOutOfBounds(Point pos, int width, int height)
    {
        return pos.X >= 0 && pos.Y >= 0 && pos.X < width && pos.Y < height;
    }
}