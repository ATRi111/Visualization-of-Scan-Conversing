using MyTimer;
using Services;
using Services.Event;
using Services.ObjectPools;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PaintController : MonoBehaviour
{
    protected IEventSystem eventSystem;
    public DraggableVertexManager vertexManager;
    [HideInInspector]
    public GridGenerator gridGenerator;

    [Range(0.1f,1f)]
    public float interval = 0.2f;

    [SerializeField]
    protected ScanTimer timer;

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gridGenerator = vertexManager.GetComponentInChildren<GridGenerator>();
        timer = CreateTimer();
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.AddListener(EEvent.AfterReset, AfterReset);
    }
    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.RemoveListener(EEvent.AfterReset, AfterReset);
    }

    public void AfterLaunch()
    {
        timer.Initialize(interval, this);
    }
    public void AfterReset()
    {
        timer.Paused = true;
        timer.ClearAll();
    }

    public void Pause()
    {
        timer.Paused = true;
    }
    public void Continue()
    {
        if(!timer.Completed)
            timer.Paused = false;
    }

    protected abstract ScanTimer CreateTimer();
}

[Serializable]
public abstract class ScanTimer : Metronome
{
    protected PaintController controller;
    protected GridGenerator gridGenerator;
    [SerializeField]
    protected ColoringTimer coloring;

    public virtual void Initialize(float duration, PaintController controller, bool start = true)
    {
        base.Initialize(duration, start);
        this.controller = controller;
        gridGenerator = controller.GetComponentInChildren<GridGenerator>();
        BeforePause += MyBeforePause;
        coloring = new ColoringTimer();
        coloring.AfterCompelete += AfterColoring;
    }

    protected virtual void MyBeforePause(float _)
    {
        coloring.Paused = true;
    }

    protected override void MyOnComplete(float _)
    {
        
    }

    protected void AfterColoring(float _)
    {
        Paused = false;
    }

    public void ClearAll()
    {
        foreach (Grid grid in gridGenerator.grids.Values)
        {
            grid.Color = Color.white;
        }
    }
}
[Serializable]
public class ColoringTimer : Metronome
{
    private List<Vector2Int> positions;
    private List<Color> colors;
    private GridGenerator gridGenerator;
    private IMyObject gridMark;
    private int index;

    public void Initialize(GridGenerator gridGenerator, List<Vector2Int> positions, List<Color> colors, float duration, bool start = true)
    {
        base.Initialize(duration, start);
        this.positions = positions;
        this.colors = colors;
        this.gridGenerator = gridGenerator;
        index = 0;
        gridMark = ServiceLocator.Get<IObjectManager>().Activate("GridMark", new Vector3(-114, -514), Vector3.zero);
    }

    protected override void MyOnComplete(float _)
    {
        if (index > positions.Count)
        {
            gridMark?.Recycle();
            return;
        }

        Vector2Int v = positions[index];
        gridMark.Transform.position = new Vector3(v.x, v.y);
        gridGenerator[v].Color = colors[index];
        index++;
        base.MyOnComplete(_);
    }
}