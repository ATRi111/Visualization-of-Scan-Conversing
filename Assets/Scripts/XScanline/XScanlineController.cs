using System.Collections.Generic;
using UnityEngine;

public class XScanlineController : PaintController
{
    protected override ScanTimer CreateTimer()
    {
        return new XScanlineTimer();
    }
}

public class XScanlineTimer : ScanTimer
{
    enum EState
    {
        DrawIntersection,   //绘制交点
        Coloring,           //多边形内区域填色
        ClearIntersection   //清除交点
    }

    private XScanlineAlgorithm algorithm;
    private EState state;

    public override void Initialize(float duration, PaintController controller, bool start = true)
    {
        base.Initialize(duration, controller, start);
        algorithm = new XScanlineAlgorithm(controller.vertexManager.Positions);
        state = EState.ClearIntersection;
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
                coloring.Initialize(gridGenerator, points, colors, Duration);
                state = EState.ClearIntersection;
                base.MyOnComplete(_);
                Paused = true;
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