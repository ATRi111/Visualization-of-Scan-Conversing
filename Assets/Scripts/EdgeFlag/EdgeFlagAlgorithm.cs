using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EdgeFlagAlgorithm
{
    public int left, down, right, up;
    public List<Vector2Int> discreted;
    public List<Vector2Int> currentLine;
    public List<Color> currentColors;

    public EdgeFlagAlgorithm(Vector3[] positions)
    {
        left = down = int.MaxValue;
        right = up = int.MinValue;
        for (int i = 0; i < positions.Length; i++)
        {
            left = Mathf.Min(left, Mathf.FloorToInt(positions[i].x));
            right = Mathf.Max(right,Mathf.CeilToInt(positions[i].x));
            down = Mathf.Min(down, Mathf.CeilToInt(positions[i].y));
            up = Mathf.Max(up, Mathf.CeilToInt(positions[i].y));
            Discrete(positions[i], positions[(i + 1) % positions.Length]);
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
            discreted.Add(new Vector2Int(edge.CurrentX, y));
        }
    }
}
