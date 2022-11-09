using System.Collections.Generic;
using Nez;

namespace lunge.Library.AI.Pathfinding
{
    public enum CoordinatesType
    {
        Tile,
        Absolute
    }

    public interface IPathfinder
    {
        string Alias { get; }
        CoordinatesType CoordinatesType { get; }
        void Initialize();
        void Find();
        void Render(Batcher batcher);
    }

    public interface IPathfinder<T> : IPathfinder where T : struct
    {
        T Start { get; set; }
        T End { get; set; }
        IEnumerable<T> Nodes { get; }
    }
}