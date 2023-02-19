using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;

namespace lunge.Library.AI;

public class QuadTreeNode<T> where T : IHasRect
{
    private RectangleF _bounds;
    private List<QuadTreeNode<T>> _nodes = new ();

    public bool IsEmpty => _bounds.IsEmpty || _nodes.Count == 0;

    public RectangleF Bounds => _bounds;

    public int MinSize = 10;

    public int Count
    {
        get
        {
            int count = 0;

            foreach (var node in _nodes)
                count += node.Count;

            count += Content.Count;

            return count;
        }
    }

    public List<T> SubTreeContent
    {
        get
        {
            List<T> result = new List<T>();

            foreach (var node in _nodes)
            {
                result.AddRange(node.SubTreeContent);
            }

            result.AddRange(Content);
            return result;
        }
    }

    public List<T> Content { get; } = new ();

    public QuadTreeNode(RectangleF bounds)
    {
        _bounds = bounds;
    }

    public List<T> Query(RectangleF queryArea)
    {
        var result = new List<T>();

        foreach (var item in Content)
        {
            if (queryArea.Intersects(item.Rectangle))
                result.Add(item);
        }

        foreach (var node in _nodes)
        {
            if (node.IsEmpty)
                continue;

            if (node.Bounds.Contains(queryArea))
            {
                result.AddRange(node.Query(queryArea));
                break;
            }

            if (queryArea.Contains(node.Bounds))
            {
                result.AddRange(node.SubTreeContent);
                continue;
            }

            if (node.Bounds.Intersects(queryArea))
            {
                result.AddRange(node.Query(queryArea));
            }
        }

        return result;
    }

    public void Insert(T item)
    {
        if (!_bounds.Contains(item.Rectangle))
        {
            return;
        }

        if (_nodes.Count == 0)
            CreateSubNodes();

        foreach (var node in _nodes)
        {
            if (node.Bounds.Contains(item.Rectangle))
            {
                node.Insert(item);
                return;
            }
        }

        Content.Add(item);
    }

    public void ForEach(QuadTree<T>.QtAction action)
    {
        action(this);

        foreach (var node in _nodes)
        {
            node.ForEach(action);
        }
    }

    private void CreateSubNodes()
    {
        if ((_bounds.Height * _bounds.Width) <= MinSize)
            return;

        var halfW = _bounds.Width / 2f;
        var halfH = _bounds.Height / 2f;

        _nodes.Add(new QuadTreeNode<T>(new RectangleF(_bounds.Location, new Vector2(halfW, halfH))));
        _nodes.Add(new QuadTreeNode<T>(new RectangleF(_bounds.Left, _bounds.Top + halfH, halfW, halfH)));
        _nodes.Add(new QuadTreeNode<T>(new RectangleF(_bounds.Left + halfW, _bounds.Top, halfW, halfH)));
        _nodes.Add(new QuadTreeNode<T>(new RectangleF(_bounds.Left + halfW, _bounds.Top + halfH, halfW, halfH)));
    }
}