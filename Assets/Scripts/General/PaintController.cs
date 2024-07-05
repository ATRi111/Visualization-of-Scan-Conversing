using MyTimer;
using Services;
using Services.Event;
using Services.ObjectPools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PaintController : MonoBehaviour
{
    protected IEventSystem eventSystem;
    public DraggableVertexManager vertexManager;
    [HideInInspector]
    public GridGenerator gridGenerator;

    public Color color_answer;
    public Color color_mark;

    public float interval = 0.2f;

    [SerializeField]
    protected ScanTimer timer;

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        timer = new ScanTimer();
        gridGenerator = vertexManager.GetComponentInChildren<GridGenerator>();
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
}

[Serializable]
public class ScanTimer : Metronome
{
    protected PaintController controller;
    protected GridGenerator gridGenerator;
    [SerializeField]
    private PaintLineTimer line;

    public virtual void Initialize(float duration, PaintController controller)
    {
        base.Initialize(duration);
        this.controller = controller;
        gridGenerator = controller.GetComponentInChildren<GridGenerator>();
        BeforePause += MyBeforePause;
        line = new PaintLineTimer();
        line.AfterCompelete += AfterPaintLine;
    }

    protected virtual void MyBeforePause(float _)
    {
        line.Paused = true;
    }

    protected override void MyOnComplete(float _)
    {
        
    }

    protected void AfterPaintLine(float _)
    {
        ForceComplete();
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
public class PaintLineTimer : Metronome
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