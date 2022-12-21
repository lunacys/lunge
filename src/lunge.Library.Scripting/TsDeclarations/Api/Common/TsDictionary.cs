namespace lunge.Library.Scripting.TsDeclarations.Api.Common;

[TsClass(Name = "Dictionary")]
public class TsDictionary<TKey, TValue> where TKey : notnull
{
    [TsExclude] internal Dictionary<TKey, TValue> OrigDictionary { get; set; } = null!;

    [TsExclude]
    internal TsDictionary(Dictionary<TKey, TValue> origDict)
    {
        OrigDictionary = origDict;
    }

    public TsDictionary()
    {
        OrigDictionary = new Dictionary<TKey, TValue>();
    }

    public int Count => OrigDictionary.Count;

    public void Add(TKey key, TValue value) => OrigDictionary.Add(key, value);
    public TValue Get(TKey key) => OrigDictionary[key];
    public void Clear() => OrigDictionary.Clear();
    public bool ContainsKey(TKey key) => OrigDictionary.ContainsKey(key);
    public bool ContainsValue(TValue value) => OrigDictionary.ContainsValue(value);
}