using System;
using System.Collections.Generic;
using UnityEngine;

public static class BezierCurve
{
    public static void CalculateNext(List<Vector3> points, float t)
    {
        if (points.Count <= 1)
            throw new ArgumentException();
        for (int i = 0; i < points.Count - 1; i++)
        {
            points[i] = Vector3.Lerp(points[i], points[i + 1], t);
        }
        points.RemoveAt(points.Count - 1);
    }
}