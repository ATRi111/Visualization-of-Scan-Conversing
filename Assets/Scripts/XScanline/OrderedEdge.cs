using System;
using UnityEngine;

[Serializable]
public class OrderedEdge : IComparable<OrderedEdge>
{
    public static OrderedEdge TryCreateOrderedEdge(Vector3 from,Vector3 to)
    {
        if (from.y > to.y)
            (from, to) = (to, from);
        int yMin = Mathf.CeilToInt(from.x);
        int yMax = Mathf.FloorToInt(to.x) - 1;
        if (yMax < yMin)
            return null;
        float deltaX = (to.x - from.x) / (to.y - from.y);
        float currentX = from.x - deltaX;
        return new OrderedEdge(yMax, currentX, deltaX);
    }

    public int yMax;

    public float currentX;
    public float deltaX;

    private OrderedEdge(int yMax, float currentX, float deltaX)
    {
        this.yMax = yMax;
        this.currentX = currentX;
        this.deltaX = deltaX;
    }

    /// <summary>
    /// 使扫描线上移，然后返回交点的x分量
    /// </summary>
    public int MoveUp()
    {
        currentX += deltaX;
        return Mathf.RoundToInt(currentX);
    }

    public int CompareTo(OrderedEdge other)
    {
        if (currentX < other.currentX)
            return -1;
        if (currentX > other.currentX)
            return 1;
        if(deltaX <  other.deltaX)
            return -1;
        if(deltaX > other.deltaX)
            return 1;
        return 0;
    }
}