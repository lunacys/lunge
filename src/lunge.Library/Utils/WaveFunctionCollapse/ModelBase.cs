using System;

namespace lunge.Library.Utils.WaveFunctionCollapse;

public abstract class ModelBase
{
    public enum Heuristic { Entropy, MRV, Scanline };

    protected bool[][]? Wave;

    protected int[][][] Propagator = null!;
    private int[][][] compatible = null!;
    protected int[] Observed = null!;

    private (int, int)[] stack = null!;
    private int stacksize, observedSoFar;

    protected int MX, MY, T, N;
    protected bool Periodic, Ground;

    protected double[] Weights = null!;
    private double[] weightLogWeights = null!, distribution = null!;

    protected int[] SumsOfOnes = null!;
    private double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
    protected double[] SumsOfWeights = null!, SumsOfWeightLogWeights = null!, Entropies = null!;

    private readonly Heuristic _heuristic;

    protected static int[] Dx = { -1, 0, 1, 0 };
    protected static int[] Dy = { 0, 1, 0, -1 };
    private static readonly int[] Opposite = { 2, 3, 0, 1 };

    protected ModelBase(int width, int height, int n, bool periodic, Heuristic heuristic)
    {
        MX = width;
        MY = height;
        N = n;
        Periodic = periodic;
        _heuristic = heuristic;
    }

    public bool Run(int seed, int limit)
    {
        if (Wave == null) 
            Init();

        Clear();
        Random random = new(seed);

        for (int l = 0; l < limit || limit < 0; l++)
        {
            int node = NextUnobservedNode(random);
            if (node >= 0)
            {
                Observe(node, random);
                bool success = Propagate();
                if (!success) return false;
            }
            else
            {
                for (int i = 0; i < Wave.Length; i++) for (int t = 0; t < T; t++) if (Wave[i][t]) { Observed[i] = t; break; }
                return true;
            }
        }

        return true;
    }

    private void Init()
    {
        Wave = new bool[MX * MY][];
        compatible = new int[Wave.Length][][];
        for (int i = 0; i < Wave.Length; i++)
        {
            Wave[i] = new bool[T];
            compatible[i] = new int[T][];
            for (int t = 0; t < T; t++) compatible[i][t] = new int[4];
        }
        distribution = new double[T];
        Observed = new int[MX * MY];

        weightLogWeights = new double[T];
        sumOfWeights = 0;
        sumOfWeightLogWeights = 0;

        for (int t = 0; t < T; t++)
        {
            weightLogWeights[t] = Weights[t] * Math.Log(Weights[t]);
            sumOfWeights += Weights[t];
            sumOfWeightLogWeights += weightLogWeights[t];
        }

        startingEntropy = Math.Log(sumOfWeights) - sumOfWeightLogWeights / sumOfWeights;

        SumsOfOnes = new int[MX * MY];
        SumsOfWeights = new double[MX * MY];
        SumsOfWeightLogWeights = new double[MX * MY];
        Entropies = new double[MX * MY];

        stack = new (int, int)[Wave.Length * T];
        stacksize = 0;
    }
    private int NextUnobservedNode(Random random)
    {
        if (Wave == null)
            throw new NullReferenceException("Wave cannot be null");

        if (_heuristic == Heuristic.Scanline)
        {
            for (int i = observedSoFar; i < Wave.Length; i++)
            {
                if (!Periodic && (i % MX + N > MX || i / MX + N > MY)) continue;
                if (SumsOfOnes[i] > 1)
                {
                    observedSoFar = i + 1;
                    return i;
                }
            }
            return -1;
        }

        double min = 1E+4;
        int argmin = -1;
        for (int i = 0; i < Wave.Length; i++)
        {
            if (!Periodic && (i % MX + N > MX || i / MX + N > MY)) continue;
            int remainingValues = SumsOfOnes[i];
            double entropy = _heuristic == Heuristic.Entropy ? Entropies[i] : remainingValues;
            if (remainingValues > 1 && entropy <= min)
            {
                double noise = 1E-6 * random.NextDouble();
                if (entropy + noise < min)
                {
                    min = entropy + noise;
                    argmin = i;
                }
            }
        }
        return argmin;
    }

    private void Observe(int node, Random random)
    {
        if (Wave == null)
            throw new NullReferenceException("Wave cannot be null");

        bool[] w = Wave[node];
        for (int t = 0; t < T; t++) distribution[t] = w[t] ? Weights[t] : 0.0;
        int r = distribution.Random(random.NextDouble());
        for (int t = 0; t < T; t++) if (w[t] != (t == r)) Ban(node, t);
    }

    private bool Propagate()
    {
        while (stacksize > 0)
        {
            (int i1, int t1) = stack[stacksize - 1];
            stacksize--;

            int x1 = i1 % MX;
            int y1 = i1 / MX;

            for (int d = 0; d < 4; d++)
            {
                int x2 = x1 + Dx[d];
                int y2 = y1 + Dy[d];
                if (!Periodic && (x2 < 0 || y2 < 0 || x2 + N > MX || y2 + N > MY)) continue;

                if (x2 < 0) x2 += MX;
                else if (x2 >= MX) x2 -= MX;
                if (y2 < 0) y2 += MY;
                else if (y2 >= MY) y2 -= MY;

                int i2 = x2 + y2 * MX;
                int[] p = Propagator[d][t1];
                int[][] compat = compatible[i2];

                for (int l = 0; l < p.Length; l++)
                {
                    int t2 = p[l];
                    int[] comp = compat[t2];

                    comp[d]--;
                    if (comp[d] == 0) Ban(i2, t2);
                }
            }
        }

        return SumsOfOnes[0] > 0;
    }

    private void Ban(int i, int t)
    {
        if (Wave == null)
            throw new NullReferenceException("Wave cannot be null");

        Wave[i][t] = false;

        int[] comp = compatible[i][t];
        for (int d = 0; d < 4; d++) comp[d] = 0;
        stack[stacksize] = (i, t);
        stacksize++;

        SumsOfOnes[i] -= 1;
        SumsOfWeights[i] -= Weights[t];
        SumsOfWeightLogWeights[i] -= weightLogWeights[t];

        double sum = SumsOfWeights[i];
        Entropies[i] = Math.Log(sum) - SumsOfWeightLogWeights[i] / sum;
    }

    private void Clear()
    {
        if (Wave == null)
            throw new NullReferenceException("Wave cannot be null");

        for (int i = 0; i < Wave.Length; i++)
        {
            for (int t = 0; t < T; t++)
            {
                Wave[i][t] = true;
                for (int d = 0; d < 4; d++) compatible[i][t][d] = Propagator[Opposite[d]][t].Length;
            }

            SumsOfOnes[i] = Weights.Length;
            SumsOfWeights[i] = sumOfWeights;
            SumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
            Entropies[i] = startingEntropy;
            Observed[i] = -1;
        }
        observedSoFar = 0;

        if (Ground)
        {
            for (int x = 0; x < MX; x++)
            {
                for (int t = 0; t < T - 1; t++) Ban(x + (MY - 1) * MX, t);
                for (int y = 0; y < MY - 1; y++) Ban(x + y * MX, T - 1);
            }
            Propagate();
        }
    }

    public abstract void Save(string filename);
}