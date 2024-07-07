using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveEdgeContainer
{
    [SerializeField]
    private List<OrderedEdge> edges;
    private OrderedEdgeContainer orderedEdgeContainer;
    public int currentY;

    public ActiveEdgeContainer(OrderedEdgeContainer orderedEdgeContainer)
    {
        edges = new();
        this.orderedEdgeContainer = orderedEdgeContainer;
        currentY = orderedEdgeContainer.yMin - 1;
    }

    /// <summary>
    /// 使扫描线上移，然后返回当前扫描线扫过的点
    /// </summary>
    /// <param name="ret">接收结果</param>
    public bool MoveUp(List<Vector2Int> ret)
    {
        currentY++;
        for (int i = 0;i < edges.Count;i++)
        {
            if (currentY > edges[i].yMax)
            {
                edges.RemoveAt(i);
                i--;
            }    
        }
        orderedEdgeContainer.GetCurrentEdges(currentY, edges);
        if (edges.Count == 0)
            return false;

        for (int i = 0; i < edges.Count; i++)
        {
            edges[i].MoveUp();
        }
        edges.Sort();
        for (int i = 0; i < edges.Count; i++)
        {
            ret.Add(new Vector2Int(Mathf.RoundToInt(edges[i].currentX), currentY));
        }
        return true;
    }
}
