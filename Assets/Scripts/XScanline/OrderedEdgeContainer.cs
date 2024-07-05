using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OrderedEdgeContainer
{
    private readonly Dictionary<int, List<OrderedEdge>> edges = new();
    internal int yMin, yMax;

    internal OrderedEdgeContainer(Vector3[] positions)
    {
        yMin = int.MaxValue;
        yMax = int.MinValue;
        Vector3 from, to;
        for (int i = 0; i < positions.Length; i++)
        {
            from = positions[i];
            to = positions[(i + 1) % positions.Length];
            yMin = Mathf.Min(yMin, Mathf.CeilToInt(from.y));
            yMax = Mathf.Max(yMax, Mathf.FloorToInt(from.y));
            if (from.y > to.y)
                (from, to) = (to, from);
            if (from.y != to.y)
            {
                OrderedEdge edge = new(from, to);
                Add(Mathf.CeilToInt(from.y), edge);
            }
        }
    }

    internal void Add(int yMin, OrderedEdge edge)
    {
        if (!edges.ContainsKey(yMin))
            edges.Add(yMin, new List<OrderedEdge>());
        edges[yMin].Add(edge);
    }

    internal void GetCurrentEdges(int y, List<OrderedEdge> ret)
    {
        if (edges.ContainsKey(y))
        {
            for (int i = 0; i < edges[y].Count; i++)
            {
                ret.Add(edges[y][i]);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        foreach (List<OrderedEdge> list in edges.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendLine(list[i].ToString());
            }
        }
        return sb.ToString();
    }
}
