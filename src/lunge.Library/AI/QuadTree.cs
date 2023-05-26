using System.Collections.Generic;
using Nez;

namespace lunge.Library.AI;

public class QuadTree<T> where T : IHasRect
{
    private readonly QuadTreeNode<T> _root;
    private RectangleF _rect;

    public int Count => _root.Count;

    public delegate void QtAction(QuadTreeNode<T> obj);

    public QuadTree(RectangleF rect)
    {
        _rect = rect;
        _root = new QuadTreeNode<T>(_rect);
    }

    public void Insert(T item)
    {
        _root.Insert(item);
    }

    public List<T> Query(RectangleF area)
    {
        return _root.Query(area);
    }

    public void ForEach(QtAction action)
    {
        _root.ForEach(action);
    }
}