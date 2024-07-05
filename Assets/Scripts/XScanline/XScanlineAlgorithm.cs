using System.Collections.Generic;
using UnityEngine;

public class XScanlineAlgorithm
{
    public List<Vector2Int> current;    //当前交点
    private readonly OrderedEdgeContainer orderedEdges;
    private readonly ActiveEdgeContainer activeEdges;

    public XScanlineAlgorithm(Vector3[] positions)
    {
        current = new List<Vector2Int>();
        orderedEdges =  new OrderedEdgeContainer(positions);
        activeEdges = new ActiveEdgeContainer(orderedEdges);
    }

    public bool CalculateNext()
    {
        current.Clear();
        return activeEdges.MoveUp(current);
    }
}
