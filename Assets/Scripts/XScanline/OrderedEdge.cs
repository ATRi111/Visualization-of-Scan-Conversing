using System;
using UnityEngine;

public class OrderedEdge : IComparable<OrderedEdge>
{
    public int yMax;

    private float currentX;
    internal float deltaX;

    internal OrderedEdge(Vector3 a, Vector3 b)
    {
        deltaX = (b.x - a.x) / (b.y - a.y);
        currentX = a.x - deltaX;
        yMax = Mathf.FloorToInt(b.y) - 1;
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