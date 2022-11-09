namespace lunge.DtsGenerator.TestTypes;

public class TestCollectionOne<T1>
{
    private T1[] _values;

    public static int DefaultSize => 16;

    public int Size { get; }

    public TestCollectionOne() : this(DefaultSize)
    { }

    public TestCollectionOne(int size)
    {
        Size = size;
        _values = new T1[Size];
    }

    public T1 Get(int index) => _values[index];
    public void Set(int index, T1 value) => _values[index] = value;

    public T1 this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }

    public void Clear()
    {
        _values = new T1[Size];
    }
}

public class TestCollectionTwo<TKey, TValue>
{
    private Dictionary<TKey, TValue> _values = new Dictionary<TKey, TValue>();

    public int Field1;
    public bool Field2;
    public string? Field3;

    public readonly string? ReadonlyField = "a";

    public static void TestStaticMethod(int a)
    { }

    public static int TestStaticMethod2(int a, string b)
    {
        return a;
    }

    public TestCollectionTwo()
    { }

    public TValue Get(TKey key) => _values[key];
    public void Set(TKey key, TValue value) => _values[key] = value;

    public void AddRange(IEnumerable<TKey> keys, IEnumerable<TValue> values) {}

    public void AddRangeArray(TKey[] keys, TValue[] values) { }

    public IEnumerable<TValue> GetRange(IEnumerable<TKey> keys)
    {
        return new TValue[] { _values[keys.First()] };
    }

    public TValue[] GetRangeArray(TKey[] keys)
    {
        return new TValue[] { _values[keys.First()] };
    }
}

public class TestCollectionThree<T1, T2, T3>
{

}

public class TestConstraintClass
{
    public string? Name { get; set; }
}

public class TestCollectionConstraints1<T1> where T1 : TestConstraintClass
{
    private T1[] _values;

    public static int DefaultSize => 16;

    public int Size { get; }

    public TestCollectionConstraints1() : this(DefaultSize)
    { }

    public TestCollectionConstraints1(int size)
    {
        Size = size;
        _values = new T1[Size];
    }

    public T1 Get(int index) => _values[index];
    public void Set(int index, T1 value) => _values[index] = value;

    public string? GetNameOf(int index) => _values[index].Name;

    public T1 this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }

    public void Clear()
    {
        _values = new T1[Size];
    }
}