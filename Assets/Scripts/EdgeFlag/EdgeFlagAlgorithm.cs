using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EdgeFlagAlgorithm
{
    public int xMin, yMin, xMax, yMax;
    public List<Vector2Int> dicreted;
    public List<Vector2Int> currentLine;
    public List<Color> currentColors;

    public EdgeFlagAlgorithm(Vector3[] positions)
    {
        xMin = yMin = int.MaxValue;
        xMax = yMax = int.MinValue;
        for (int i = 0; i < positions.Length; i++)
        {

        }
    }

    private void Discrete(Vector3 from,Vector3 to)
    {

    }
}
