using System;
using System.Collections.Generic;
using UnityEngine;

public class XScanlineController : PaintController<XScanlineTimer>
{

}

[Serializable]
public class XScanlineTimer : ScanTimer
{
    public enum EState
    {
        DrawIntersection,   //绘制交点
        Coloring,           //多边形内区域填色
        ClearIntersection   //清除交点
    }

    [SerializeField]
    private XScanlineAlgorithm algorithm;
    [SerializeField]
    private EState state;

    public override void Initialize(float duration, Vector3[] positions, GridGenerator gridGenerator, bool start = true)
    {
        base.Initialize(duration, positions, gridGenerator, start);
        algorithm = new XScanlineAlgorithm(positions);
        state = EState.DrawIntersection;
    }

    protected override void MyOnComplete(float _)
    {
        switch(state)
        {
            case EState.DrawIntersection:
                if(algorithm.CalculateNext())
                {
                    for (int i = 0; i < algorithm.current.Count; i++)
                    {
                        gridGenerator[algorithm.current[i]].Color = Color.blue;
                    }
                    state = EState.Coloring;
                    base.MyOnComplete(_);
                }
                else
                    return;
                break;
            case EState.Coloring:
                List<Vector2Int> points = new();
                List<Color> colors = new();
                for (int i = 0; i < algorithm.current.Count; i+= 2)
                {
                    Vector2Int from = algorithm.current[i];
                    Vector2Int to = algorithm.current[i + 1];
                    for (int j = from.x; j < to.x;j ++) //左闭右开
                    {
                        points.Add(new Vector2Int(j, from.y));
                        colors.Add(Color.red);
                    }
                }
                state = EState.ClearIntersection;
                base.MyOnComplete(_);
                Paused = true;
                coloring.Initialize(gridGenerator, points, colors, Duration);
                break;
            case EState.ClearIntersection:
                for (int i = 0; i < algorithm.current.Count; i++)
                {
                    Vector2Int v = algorithm.current[i];
                    if (gridGenerator[v].Color == Color.blue)
                        gridGenerator[algorithm.current[i]].Color = Color.white;
                }
                state = EState.DrawIntersection;
                base.MyOnComplete(_);
                break;
        }
    }
}