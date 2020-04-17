namespace lunge.Library
{
    public class CircularArray<T>
    {
        private int _start;

        public int Start
        {
            get => _start;
            set => _start = value % _array.Length;
        }

        public int Count { get; set; }
        public int Capacity => _array.Length;

        private readonly T[] _array;

        public CircularArray(int capacity)
        {
            _array = new T[capacity];    
        }

        public T this[int index]
        {
            get => _array[(_start + index) % _array.Length];
            set => _array[(_start + index) % _array.Length] = value;
        }
    }
}