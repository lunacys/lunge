using System;
using System.Collections.Generic;
using System.Linq;
using lunge.Library;
using lunge.Library.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Textures;

namespace Playground.Components.BitmaskTests;

public class BitmaskTestsComponent : RenderableComponent, IUpdatable
{
    public override float Width => 1024;
    public override float Height => 1024;

    public static readonly int TileSize = 32;

    private List<Sprite> _tileSet;
    private List<Sprite> _tileSetDiag;

    private bool[,] _bitMap = null!;

    private int[,] _bitMask = null!;

    private int[,] _bitMaskDiag = null!;

    public int WidthTiles => (int)Width / TileSize;
    public int HeightTiles => (int)Height / TileSize;

    private Camera _camera;

    private readonly Dictionary<int, int> _lookupTable = new ();

    private HashSet<int> _allCapturedVals = new();

    public BitmaskTestsComponent(List<Sprite> tileset, List<Sprite> tilesetDiag)
    {
        _tileSet = tileset;
        _tileSetDiag = tilesetDiag;

        //for (int i = 0; i < 10000; i++)
        {
            GenerateRandomBitmap();
        }

        var l = _allCapturedVals.ToList();
        l.Sort();
        //Console.WriteLine(string.Join(", ", l));

        //Console.WriteLine("Total: " + l.Count);

        var lookUpTable = CreateLookUpTable();

        var list = lookUpTable.ToList();
        list.Sort((p1, p2) =>
        {
            return p1.Value.CompareTo(p2.Value);
        });

        Console.WriteLine($"Lookup Key Count: " + lookUpTable.Count);
        foreach (var kv in list)
        {
            Console.WriteLine($"{{ {kv.Key}, {kv.Value} }}");
        }

        _lookupTable = lookUpTable;
    }

    public override void OnAddedToEntity()
    {
        _camera = Core.Scene.Camera;
    }

    public void Update()
    {
        if (Input.IsKeyPressed(Keys.Space))
            GenerateRandomBitmap();

        if (Input.LeftMouseButtonDown)
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.MousePosition);
            var mousePosPoint = new Point(
                (int)mousePos.X / TileSize,
                (int)mousePos.Y / TileSize
            );

            if (IsNotOutOfBounds(mousePosPoint))
            {
                _bitMap[mousePosPoint.Y, mousePosPoint.X] = true;
                RecalculateBitmasks();
            }
        }

        if (Input.RightMouseButtonDown)
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.MousePosition);
            var mousePosPoint = new Point(
                (int)mousePos.X / TileSize,
                (int)mousePos.Y / TileSize
            );

            if (IsNotOutOfBounds(mousePosPoint))
            {
                _bitMap[mousePosPoint.Y, mousePosPoint.X] = false;
                RecalculateBitmasks();
            }
        }
    }

    public override void Render(Batcher batcher, Camera camera)
    {
        batcher.DrawHollowRect(0, 0, Width, Height, Color.White);

        for (int y = 0; y < Height / TileSize; y++)
        {
            for (int x = 0; x < Width / TileSize; x++)
            {
                var pos = new Vector2(x * TileSize, y * TileSize);
                var bit = _bitMap[y, x];

                if (bit)
                {
                    batcher.DrawRect(pos, TileSize, TileSize, Color.LightGreen);
                }
                //batcher.DrawHollowRect(pos, TileSize, TileSize, Color.LightGray);

                
                if (bit)
                {
                    var bitStr = bit ? "1" : "0";
                    var mask = _bitMaskDiag[y, x];
                    var maskStr = Convert.ToString(mask, 2)
                        .PadLeft(8, '0')
                        .Insert(4, "\n");

                    batcher.DrawSprite(MapBits(mask), pos);
                    var index = _lookupTable[mask];
                    batcher.DrawString(Graphics.Instance.BitmapFont, $"{index}", pos + new Vector2(2, 2), Color.Black);
                    batcher.DrawString(Graphics.Instance.BitmapFont, $"{maskStr}", pos + new Vector2(6, 12), Color.Red);
                }
            }
        }
    }

    private Sprite MapBits(int bits)
    {
        var index = _lookupTable[bits];

        return _tileSetDiag[index];
    }

    private Dictionary<int, int> CreateLookUpTable()
    {
        var result = new Dictionary<int, int>();

        for (int i = 0; i < 256; i++)
        {
            var dcba = i & 0b1111_0000;
            var diag = i & 0b0000_1111;

            switch (dcba)
            {
                case 0b0000_0000:
                    if (diag == 0b0000_0000)
                        result[i] = 47;
                    else if (diag == 0b0000_0001)
                        result[i] = 44;
                    else if (diag == 0b0000_0010)
                        result[i] = 36;
                    else if (diag == 0b0000_0011)
                        result[i] = 43; 
                    else if (diag == 0b0000_0100)
                        result[i] = 37;
                    else if (diag == 0b0000_0101)
                        result[i] = 14;
                    else if (diag == 0b0000_0110)
                        result[i] = 35;
                    else if (diag == 0b0000_0111)
                        result[i] = 32;
                    else if (diag == 0b0000_1000)
                        result[i] = 45;
                    else if (diag == 0b0000_1001)
                        result[i] = 34;
                    else if (diag == 0b0000_1010)
                        result[i] = 15;
                    else if (diag == 0b0000_1011)
                        result[i] = 40;
                    else if (diag == 0b0000_1100)
                        result[i] = 42;
                    else if (diag == 0b0000_1101)
                        result[i] = 41;
                    else if (diag == 0b0000_1110)
                        result[i] = 33;
                    else if (diag == 0b0000_1111)
                        result[i] = 38;
                    break;
                case 0b0001_0000: 
                    if ((diag & 0b0000_0110) == 0b0000_0000)
                        result[i] = 28;
                    else if ((diag & 0b0000_0110) == 0b0000_0010)
                        result[i] = 27;
                    else if ((diag & 0b0000_0110) == 0b0000_0100)
                        result[i] = 26;
                    else if ((diag & 0b0000_0110) == 0b0000_0110)
                        result[i] = 23;
                    break;
                case 0b0010_0000:
                    if ((diag & 0b0000_1100) == 0b0000_0000)
                        result[i] = 21;
                    else if ((diag & 0b0000_1100) == 0b0000_0100)
                        result[i] = 24;
                    else if ((diag & 0b0000_1100) == 0b0000_1000)
                        result[i] = 16;
                    else if ((diag & 0b0000_1100) == 0b0000_1100)
                        result[i] = 30;
                    break;
                case 0b0011_0000:
                    if ((diag & 0b0000_0100) == 0b0000_0000)
                        result[i] = 1;
                    if ((diag & 0b0000_0100) == 0b0000_0100)
                        result[i] = 3;
                    break;
                case 0b0100_0000:
                    if ((diag & 0b0000_1001) == 0b0000_0000)
                        result[i] = 29;
                    else if ((diag & 0b0000_1001) == 0b0000_0001)
                        result[i] = 19;
                    else if ((diag & 0b0000_1001) == 0b0000_1000)
                        result[i] = 18;
                    else if ((diag & 0b0000_1001) == 0b0000_1001)
                        result[i] = 22;
                    break;
                case 0b0101_0000:
                    result[i] = 7;
                    break;
                case 0b0110_0000:
                    if ((diag & 0b0000_1000) == 0b0000_0000)
                        result[i] = 9;
                    else if ((diag & 0b0000_1000) == 0b0000_1000)
                        result[i] = 11;
                    break;
                case 0b0111_0000:
                    result[i] = 13;
                    break;
                case 0b1000_0000:
                    if ((diag & 0b0000_0011) == 0b0000_0000)
                        result[i] = 20;
                    else if ((diag & 0b0000_0011) == 0b0000_0001)
                        result[i] = 17;
                    else if ((diag & 0b0000_0011) == 0b0000_0010)
                        result[i] = 25;
                    else if ((diag & 0b0000_0011) == 0b0000_0011)
                        result[i] = 31;
                    break;
                case 0b1001_0000:
                    if ((diag & 0b0000_0010) == 0b0000_0000)
                        result[i] = 0;
                    else if ((diag & 0b0000_0010) == 0b0000_0010)
                        result[i] = 2;
                    break;
                case 0b1010_0000:
                    result[i] = 6;
                    break;
                case 0b1011_0000:
                    result[i] = 5;
                    break;
                case 0b1100_0000:
                    if ((diag & 0b0000_0001) == 0b0000_0000)
                        result[i] = 8;
                    else if ((diag & 0b0000_0001) == 0b0000_0001)
                        result[i] = 10;
                    break;
                case 0b1101_0000:
                    result[i] = 12;
                    break;
                case 0b1110_0000:
                    result[i] = 4;
                    break;
                case 0b1111_0000:
                    result[i] = 46; 
                    break;
            }
        }

        return result;
    }

    private void GenerateRandomBitmap()
    {
        _bitMap = new bool[(int)Height / TileSize, (int)Width / TileSize];

        for (int y = 0; y < Height / TileSize; y++)
        {
            for (int x = 0; x < Width / TileSize; x++)
            {
                _bitMap[y, x] = false; //Random.Chance(0.5f);
            }
        }

        _bitMask = new int[(int)Height / TileSize, (int)Width / TileSize];
        _bitMaskDiag = new int[(int)Height / TileSize, (int)Width / TileSize];

        RecalculateBitmasks();
    }

    private void RecalculateBitmasks()
    {
        _bitMask = BitMaskHelper.CalculateBitMaskForBitMap(_bitMap, false);
        _bitMaskDiag = BitMaskHelper.CalculateBitMaskForBitMap(_bitMap, true);
    }

    bool IsNotOutOfBounds(Point pos)
    {
        return pos.X >= 0 && pos.Y >= 0 && pos.X < WidthTiles && pos.Y < HeightTiles;
    }
}