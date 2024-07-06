using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private Area2D area;
    private IObjectManager objectManager;
    [SerializeField]
    private int extend;
    public Dictionary<Vector2Int, Grid> grids;

    public Grid this[int x,int y]
    {
        get
        {
            grids.TryGetValue(new Vector2Int(x, y), out Grid grid);
            return grid;
        }
    }
    public Grid this[Vector2Int v]
    {
        get
        {
            grids.TryGetValue(v, out Grid grid);
            return grid;
        }
    }

    private void Awake()
    {
        area = GetComponent<Area2D>();
        objectManager = ServiceLocator.Get<IObjectManager>();
        grids = new Dictionary<Vector2Int, Grid>();
    }

    private void Start()
    {
        GenerateGrids();
    }

    private void GenerateGrids()
    {
        area.ShrinkToInt(out int xMin, out int xMax, out int yMin, out int yMax);
        grids.Clear();
        for (int i = xMin - extend; i <= xMax + extend; i++) 
        {
            for (int j = yMin - extend; j <= yMax + extend; j++) 
            {
                IMyObject obj = objectManager.Activate("Grid", new Vector3(i, j), Vector3.zero, transform);
                grids.Add(new Vector2Int(i, j), obj.Transform.GetComponent<Grid>());
            }
        }
    }

    public void ResetColor()
    {
        foreach(Grid grid in grids.Values)
        {
            grid.Color = Color.white;
        }
    }
}