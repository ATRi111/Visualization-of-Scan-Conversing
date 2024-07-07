using MyTimer;
using Services;
using Services.Event;
using Services.ObjectPools;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PaintController<T> : MonoBehaviour, IPaintController where T : ScanTimer, new()
{
    protected IEventSystem eventSystem;
    public DraggableVertexManager vertexManager;
    [HideInInspector]
    public GridGenerator gridGenerator;

    public float Interval { get; set; }

    public T timer;

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gridGenerator = vertexManager.GetComponentInChildren<GridGenerator>();
        timer = new T();
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

    private void AfterLaunch()
    {
        timer.Initialize(Interval, vertexManager.Positions, gridGenerator);
    }
    private void AfterReset()
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
        if (!timer.Completed)
            timer.Paused = false;
    }
}

[Serializable]
public class ScanTimer : Metronome
{
    protected GridGenerator gridGenerator;
    [SerializeField]
    protected ColoringTimer coloring;

    public virtual void Initialize(float duration, Vector3[] positions , GridGenerator gridGenerator, bool start = true)
    {
        base.Initialize(duration, start);
        this.gridGenerator = gridGenerator;
        BeforePause += MyBeforePause;
        coloring = new ColoringTimer();
        coloring.AfterColoring += AfterColoring;
    }

    protected virtual void MyBeforePause(float _)
    {
        coloring.Paused = true;
    }

    protected void AfterColoring()
    {
        Paused = false;
    }

    public void ClearAll()
    {
        gridGenerator.ResetColor();
    }
}

public class ColoringTimer : Metronome
{
    private List<Vector2Int> positions;
    private List<Color> colors;
    private GridGenerator gridGenerator;
    private IMyObject gridMark;
    private int index;
    public Action AfterColoring;

    public void Initialize(GridGenerator gridGenerator, List<Vector2Int> positions, List<Color> colors, float duration, bool start = true)
    {
        base.Initialize(duration, start);
        this.positions = positions;
        this.colors = colors;
        this.gridGenerator = gridGenerator;
        index = 0;
        gridMark = ServiceLocator.Get<IObjectManager>().Activate("GridMark", new Vector3(-114, -514), Vector3.zero);
        AfterColoring = null;
    }

    protected override void MyOnComplete(float _)
    {
        if (index >= positions.Count)
        {
            gridMark?.Recycle();
            AfterColoring?.Invoke();
            return;
        }

        Vector2Int v = positions[index];
        gridMark.Transform.position = new Vector3(v.x, v.y);
        gridGenerator[v].Color = colors[index];
        index++;
        base.MyOnComplete(_);
    }
}