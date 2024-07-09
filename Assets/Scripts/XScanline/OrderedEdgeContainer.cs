using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class OrderedEdgeContainer
{
    private readonly Dictionary<int, List<OrderedEdge>> edges = new();
    public int yMin, yMax;

    public OrderedEdgeContainer(Vector3[] positions)
    {
        yMin = int.MaxValue;
        yMax = int.MinValue;
        Vector3 from, to;
        for (int i = 0; i < positions.Length; i++)
        {
            from = positions[i];
            to = positions[(i + 1) % positions.Length];
            yMin = Mathf.Min(yMin, Mathf.RoundToInt(from.y));
            yMax = Mathf.Max(yMax, Mathf.RoundToInt(from.y));

            OrderedEdge edge = OrderedEdge.TryCreateOrderedEdge(from, to);
            if (edge != null)
                Add(edge.yMin, edge);
        }
    }

    public void Add(int yMin, OrderedEdge edge)
    {
        if (!edges.ContainsKey(yMin))
            edges.Add(yMin, new List<OrderedEdge>());
        edges[yMin].Add(edge);
    }

    public void GetCurrentEdges(int y, List<OrderedEdge> ret)
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
