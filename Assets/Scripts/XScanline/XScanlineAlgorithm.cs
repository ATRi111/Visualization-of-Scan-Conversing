using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XScanlineAlgorithm
{
    public List<Vector2Int> current;    //当前交点
    public OrderedEdgeContainer orderedEdges;
    public ActiveEdgeContainer activeEdges;

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
