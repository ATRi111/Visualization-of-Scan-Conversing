using System;
using UnityEngine;

public class EdgeFlagController : PaintController<EdgeFlagTimer>
{

}

[Serializable]
public class EdgeFlagTimer : ScanTimer
{
    [SerializeField]
    private EdgeFlagAlgorithm algorithm;
    [SerializeField]
    private int discretedIndex;

    public override void Initialize(float duration, Vector3[] positions, GridGenerator gridGenerator, bool start = true)
    {
        base.Initialize(duration, positions, gridGenerator, start);
        algorithm = new EdgeFlagAlgorithm(positions);
        discretedIndex = 0;
    }

    protected override void MyOnComplete(float _)
    {
        if(discretedIndex < algorithm.discreted.Count)
        {
            gridGenerator[algorithm.discreted[discretedIndex]].Color = Color.blue;
            discretedIndex++;
            base.MyOnComplete(_);
        }
        else
        {
            bool unfinished = algorithm.MoveUp();
            if (unfinished)
            {
                base.MyOnComplete(_);
                Paused = true;
            }
            coloring.Initialize(gridGenerator, algorithm.currentLine, algorithm.currentColors, Duration);
        }
    }
}