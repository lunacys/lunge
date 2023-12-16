using System;
using Microsoft.Xna.Framework;

namespace lunge.Library.Utils.Bitmasking;

public static class BitMaskCalculator
{
    private static Point[] _dirsEdges = new[]
    {
        new Point(0, -1), // 1, top
        new Point(1, 0), // 2, right
        new Point(0, 1), // 4, bottom
        new Point(-1, 0), // 8, left
    };

    private static Point[] _dirsCorners = new[]
    {
        new Point(1, -1), // 1, top-right
        new Point(1, 1), // 2, bottom-right
        new Point(-1, 1), // 4, bottom-left
        new Point(-1, -1), // 8, top-left
    };

    private static Point[] _dirsCornersAndEdges = new[]
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

    /// <summary>
    /// Calculates bitmask for a bit map. Bit map is an 2D array of bools.
    /// True means there's a tile, False - no tile.
    /// There are 3 types of bitmasks - Orthogonal (Edges), Diagonal (Corners) and Both.
    /// </summary>
    /// <param name="bitMask"></param>
    /// <param name="bitMap"></param>
    /// <param name="dirs"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void CalculateBitMaskForBitMap(int[,] bitMask, bool[,] bitMap, BitMaskDirection dirs)
    {
        var h = bitMap.GetLength(0);
        var w = bitMap.GetLength(1);

        var dirsToUse = GetDirsToUse(dirs);
        
        // Edges: 
        //      1
        //   +------+
        // 8 |      | 2
        //   |      |
        //   +------+
        //      4
        
        // Corners:
        //          1      
        // 8 +------+
        //   |      | 
        //   |      |
        //   +------+ 2
        //   4
        
        // Both:
        //        1   2
        // 128 +------+
        //     |      | 4
        // 64  |      |
        //     +------+ 8
        //    32   16

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                var bit = bitMap[y, x];
                int newByte = 0;

                // If bit is set to true (1) then get its neighbors and calculate bitmap
                if (bit)
                {
                    for (var i = 0; i < dirsToUse.Length; i++)
                    {
                        var dir = dirsToUse[i];
                        var p = new Point(x + dir.X, y + dir.Y);

                        if (MathUtils.IsNotOutOfBounds(p, w, h) && bitMap[p.Y, p.X])
                            newByte |= 1 << i; //(int)Math.Pow(2, i);
                    }
                }

                bitMask[y, x] = newByte;
            }
        }
    }
    
    public static int[,] CalculateBitMaskForBitMap(bool[,] bitMap, BitMaskDirection dirs)
    {
        var h = bitMap.GetLength(0);
        var w = bitMap.GetLength(1);

        var result = new int[h, w];

        CalculateBitMaskForBitMap(result, bitMap, dirs);

        return result;
    }

    public static int[,] CalculateBitMaskForBitMap(bool[,] bitMap, bool includeDiagonals = true)
    {
        var type = 
            includeDiagonals 
                ? BitMaskDirection.CornersAndEdges 
                : BitMaskDirection.Edges;
        
        return CalculateBitMaskForBitMap(bitMap, type);
    }
    
    public static bool[,] CreateBitMapFrom<T>(T[,] world, Predicate<T> predicate) where T : struct
    {
        var h = world.GetLength(0);
        var w = world.GetLength(1);

        var result = new bool[h, w];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                result[y, x] = predicate(world[y, x]);
            }
        }

        return result;
    }

    private static Point[] GetDirsToUse(BitMaskDirection dirs)
    {
        return dirs switch
        {
            BitMaskDirection.Edges => _dirsEdges,
            BitMaskDirection.Corners => _dirsCorners,
            BitMaskDirection.CornersAndEdges => _dirsCornersAndEdges,
            _ => throw new ArgumentOutOfRangeException(nameof(dirs), dirs, null)
        };
    }
}