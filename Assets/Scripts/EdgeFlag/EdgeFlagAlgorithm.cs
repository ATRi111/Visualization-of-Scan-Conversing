using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EdgeFlagAlgorithm
{
    public int xMin, yMin, xMax, yMax;

    public Dictionary<Vector2Int, Color> colorDict;

    public List<Vector2Int> currentLine;
    public List<Color> currentColors;
    public int currentY;
    
    public EdgeFlagAlgorithm(Vector3[] positions)
    {
        colorDict = new();
        currentLine = new();
        currentColors = new();
        xMin = yMin = int.MaxValue;
        xMax = yMax = int.MinValue;
        currentY = yMin - 1;
        for (int i = 0; i < positions.Length; i++)
        {
            xMin = Mathf.Min(xMin, Mathf.FloorToInt(positions[i].x));
            xMax = Mathf.Max(xMax,Mathf.CeilToInt(positions[i].x));
            yMin = Mathf.Min(yMin, Mathf.CeilToInt(positions[i].y));
            yMax = Mathf.Max(yMax, Mathf.CeilToInt(positions[i].y));
            Discrete(positions[i], positions[(i + 1) % positions.Length]);
        }
        for (int x = xMin;x <= xMax;x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                Vector2Int v = new(x, y);
                if (!colorDict.ContainsKey(v))
                    colorDict.Add(v, Color.white);
            }
        }
    }

    private void Discrete(Vector3 from,Vector3 to)
    {
        OrderedEdge edge = OrderedEdge.TryCreateOrderedEdge(from, to);
        if (edge == null)
            return;
        for (int y = edge.yMin; y <= edge.yMax; y++)
        {
            edge.MoveUp();
            Vector2Int v = new(edge.CurrentX, y);
            if (!colorDict.ContainsKey(v))
                colorDict.Add(v, Color.blue);
            else if (colorDict[v] == Color.blue)
                colorDict[v] = Color.white;
            else
                colorDict[v] = Color.blue;
        }
    }

    //令y增加，然后计算当前行各个点是否需要填色
    public bool MoveUp()
    {
        bool flag = false;
        currentY++;
        for (int x = xMin; x <= xMax; x++)
        {
            Vector2Int v = new(x, currentY);
            if (colorDict[v] == Color.blue)
                flag = !flag;
            colorDict[v] = flag ? Color.red : Color.white;
            currentLine.Add(v);
            currentColors.Add(colorDict[v]);
        }
        return currentY <= yMax;
    }
}
