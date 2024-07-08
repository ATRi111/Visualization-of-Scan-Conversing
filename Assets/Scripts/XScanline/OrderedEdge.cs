using System;
using UnityEngine;

[Serializable]
public class OrderedEdge : IComparable<OrderedEdge>
{
    public static OrderedEdge TryCreateOrderedEdge(Vector3 from, Vector3 to)
    {
        if (from.y > to.y)
            (from, to) = (to, from);
        int yMin = Mathf.RoundToInt(from.y);
        int yMax = Mathf.RoundToInt(to.y);
        if (yMax == yMin)
            return null;
        float deltaX = (to.x - from.x) / (to.y - from.y);
        float currentX = deltaX * (yMin - from.y - 1) + from.x; 
        return new OrderedEdge(yMin, yMax - 1, currentX, deltaX);   //-1恰好实现下闭上开
    }

    public int yMin;
    public int yMax;
    public float currentX;
    public float deltaX;

    private OrderedEdge(int yMin,int yMax, float currentX, float deltaX)
    {
        this.yMin = yMin;
        this.yMax = yMax;
        this.currentX = currentX;
        this.deltaX = deltaX;
    }

    /// <summary>
    /// 使扫描线上移
    /// </summary>
    public void MoveUp()
    {
        currentX += deltaX;
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