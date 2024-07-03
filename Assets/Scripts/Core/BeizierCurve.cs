using System;
using UnityEngine;

public static class BeizierCurve
{
    public static Vector3 Calculate(Vector3[] points, float t)
    {
        int count = points.Length;
        switch (count)
        {
            case 0:
            case 1:
                throw new ArgumentException();
            case 2:
                return Vector3.Lerp(points[0], points[1], t);
            default:
                Vector3[] newPoints = new Vector3[count - 1];
                for (int i = 0; i < count - 1; i++)
                {
                    newPoints[i] = Vector3.Lerp(points[i], points[i + 1], t);
                }
                return Calculate(newPoints, t);
        }
    }

    public static void CalculateNext(Vector3[] points, float t)
    {
        int count = points.Length;
        if(count <= 1)
            throw new ArgumentException();
        for (int i = 0; i < count - 1; i++)
        {
            points[i] = Vector3.Lerp(points[i], points[i + 1], t);
        }
        points[^1] = default;
    }
}