using Services;
using Services.ObjectPools;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private Area2D area;
    private IObjectManager objectManager;
    [SerializeField]
    private int extend;

    private void Awake()
    {
        area = GetComponent<Area2D>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void Start()
    {
        GenerateGrids();
    }

    private void GenerateGrids()
    {
        area.ShrinkToInt(out int xMin, out int xMax, out int yMin, out int yMax);
        for (int i = xMin - extend; i <= xMax + extend; i++) 
        {
            for (int j = yMin - extend; j <= yMax + extend; j++) 
            {
                objectManager.Activate("Grid", new Vector3(i, j), Vector3.zero, transform);
            }
        }
    }
}