using System.Collections;
using System.Collections.Generic;

namespace lunge.Library.Utils.Collections;

public class CircularArray<T> : IEnumerable<T>
{
    private T[] _array = null!;

    /// <summary>
    /// Gets or sets array's max size. WARNING: if size is changed, all array elements will be discarded
    /// </summary>
    public int MaxSize
    {
        get => _array.Length;
        set => UpdateSize(value);
    }

    public T Last => _array[_currIndex % MaxSize];

    public int Count => _count;

    internal int _currIndex;
    private int _count;

    public T this[int index] => _array[index % MaxSize];

    public CircularArray(int maxSize = 64)
    {
        MaxSize = maxSize;

        _currIndex = 0;
    }

    public void Add(T element)
    {
        _array[_currIndex++] = element;

        if (_currIndex >= MaxSize)
            _currIndex = 0;

        if (_count < MaxSize)
            _count++;
    }

    public void Clear()
    {
        _array = new T[MaxSize];
    }

    private void UpdateSize(int newSize)
    {
        _array = new T[newSize];
        _currIndex = 0;
        _count = 0;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_array).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}