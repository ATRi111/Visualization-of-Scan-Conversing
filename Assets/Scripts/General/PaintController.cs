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
    [SerializeField]
    private float interval_init;
    public float Interval { get; set; }

    public T timer;

    private bool paused_main;   //true表示被暂停的是timer，false表示被暂停的是timer.coloring

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gridGenerator = vertexManager.GetComponentInChildren<GridGenerator>();
        timer = new T();
        Interval = interval_init;
        paused_main = true;
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
        if(!timer.Paused && timer.coloring.Paused)
        {
            timer.Paused = true;
            paused_main = true;
        }
        else if(timer.Paused && !timer.coloring.Paused)
        {
            timer.coloring.Paused = true;
            paused_main = false;
        }
    }
    public void Continue()
    {
        if(paused_main)
            timer.Paused = false;
        else
            timer.coloring.Paused = false;
    }
}

[Serializable]
public class ScanTimer : Metronome
{
    protected GridGenerator gridGenerator;
    public ColoringTimer coloring;

    public virtual void Initialize(float duration, Vector3[] positions , GridGenerator gridGenerator, bool start = true)
    {
        base.Initialize(duration, start);
        this.gridGenerator = gridGenerator;
        BeforePause += MyBeforePause;
        coloring = new ColoringTimer
        {
            AfterColoring = AfterColoring
        };
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
        coloring.Clear();
        coloring.Paused = true;
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

    public void Clear()
    {
        gridMark?.Recycle();
    }
}