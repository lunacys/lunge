using System.Collections.Generic;
using System;

namespace lunge.Library.Utils.WaveFunctionCollapse;

public class OverlappingModel : ModelBase
{
    private readonly List<byte[]> _patterns;
    private readonly List<int> _colors;

    public OverlappingModel(string name, int N, int width, int height, bool periodicInput, bool periodic, int symmetry,
        bool ground, Heuristic heuristic)
        : base(width, height, N, periodic, heuristic)
    {
        var (bitmap, sx, sy) = BitmapHelper.LoadBitmap($"samples/{name}.png");
        byte[] sample = new byte[bitmap.Length];
        _colors = new List<int>();
        for (int i = 0; i < sample.Length; i++)
        {
            int color = bitmap[i];
            int k = 0;
            for (; k < _colors.Count; k++)
                if (_colors[k] == color)
                    break;
            if (k == _colors.Count) _colors.Add(color);
            sample[i] = (byte)k;
        }

        static byte[] Pattern(Func<int, int, byte> f, int N)
        {
            byte[] result = new byte[N * N];
            for (int y = 0; y < N; y++)
            for (int x = 0; x < N; x++)
                result[x + y * N] = f(x, y);
            return result;
        }

        ;
        static byte[] Rotate(byte[] p, int n) => Pattern((x, y) => p[n - 1 - y + x * n], n);
        static byte[] Reflect(byte[] p, int n) => Pattern((x, y) => p[n - 1 - x + y * n], n);

        static long Hash(byte[] p, int c)
        {
            long result = 0, power = 1;
            for (int i = 0; i < p.Length; i++)
            {
                result += p[p.Length - 1 - i] * power;
                power *= c;
            }

            return result;
        }

        ;

        _patterns = new();
        Dictionary<long, int> patternIndices = new();
        List<double> weightList = new();

        int C = _colors.Count;
        int xmax = periodicInput ? sx : sx - N + 1;
        int ymax = periodicInput ? sy : sy - N + 1;
        for (int y = 0; y < ymax; y++)
        for (int x = 0; x < xmax; x++)
        {
            byte[][] ps = new byte[8][];

            ps[0] = Pattern((dx, dy) => sample[(x + dx) % sx + (y + dy) % sy * sx], N);
            ps[1] = Reflect(ps[0], N);
            ps[2] = Rotate(ps[0], N);
            ps[3] = Reflect(ps[2], N);
            ps[4] = Rotate(ps[2], N);
            ps[5] = Reflect(ps[4], N);
            ps[6] = Rotate(ps[4], N);
            ps[7] = Reflect(ps[6], N);

            for (int k = 0; k < symmetry; k++)
            {
                byte[] p = ps[k];
                long h = Hash(p, C);
                if (patternIndices.TryGetValue(h, out int index)) weightList[index] = weightList[index] + 1;
                else
                {
                    patternIndices.Add(h, weightList.Count);
                    weightList.Add(1.0);
                    _patterns.Add(p);
                }
            }
        }

        Weights = weightList.ToArray();
        T = Weights.Length;
        this.Ground = ground;

        static bool Agrees(byte[] p1, byte[] p2, int dx, int dy, int N)
        {
            int xmin = dx < 0 ? 0 : dx, xmax = dx < 0 ? dx + N : N, ymin = dy < 0 ? 0 : dy, ymax = dy < 0 ? dy + N : N;
            for (int y = ymin; y < ymax; y++)
            for (int x = xmin; x < xmax; x++)
                if (p1[x + N * y] != p2[x - dx + N * (y - dy)])
                    return false;
            return true;
        }

        ;

        Propagator = new int[4][][];
        for (int d = 0; d < 4; d++)
        {
            Propagator[d] = new int[T][];
            for (int t = 0; t < T; t++)
            {
                List<int> list = new();
                for (int t2 = 0; t2 < T; t2++)
                    if (Agrees(_patterns[t], _patterns[t2], Dx[d], Dy[d], N))
                        list.Add(t2);
                Propagator[d][t] = new int[list.Count];
                for (int c = 0; c < list.Count; c++) Propagator[d][t][c] = list[c];
            }
        }
    }

    public override void Save(string filename)
    {
        int[] bitmap = new int[MX * MY];
        if (Observed[0] >= 0)
        {
            for (int y = 0; y < MY; y++)
            {
                int dy = y < MY - N + 1 ? 0 : N - 1;
                for (int x = 0; x < MX; x++)
                {
                    int dx = x < MX - N + 1 ? 0 : N - 1;
                    bitmap[x + y * MX] = _colors[_patterns[Observed[x - dx + (y - dy) * MX]][dx + dy * N]];
                }
            }
        }
        else
        {
            for (int i = 0; i < Wave.Length; i++)
            {
                int contributors = 0, r = 0, g = 0, b = 0;
                int x = i % MX, y = i / MX;
                for (int dy = 0; dy < N; dy++)
                for (int dx = 0; dx < N; dx++)
                {
                    int sx = x - dx;
                    if (sx < 0) sx += MX;

                    int sy = y - dy;
                    if (sy < 0) sy += MY;

                    int s = sx + sy * MX;
                    if (!Periodic && (sx + N > MX || sy + N > MY || sx < 0 || sy < 0)) continue;
                    for (int t = 0; t < T; t++)
                        if (Wave[s][t])
                        {
                            contributors++;
                            int argb = _colors[_patterns[t][dx + dy * N]];
                            r += (argb & 0xff0000) >> 16;
                            g += (argb & 0xff00) >> 8;
                            b += argb & 0xff;
                        }
                }

                bitmap[i] = unchecked((int)0xff000000 | ((r / contributors) << 16) | ((g / contributors) << 8) |
                                      b / contributors);
            }
        }

        BitmapHelper.SaveBitmap(bitmap, MX, MY, filename);
    }
}