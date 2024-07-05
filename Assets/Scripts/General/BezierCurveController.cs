using MyTimer;
using Services;
using Services.Event;
using Services.ObjectPools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveController : MonoBehaviour
{
    private IEventSystem eventSystem;
    public DraggableVertexManager vertexManager;

    public Color color_answer;
    public Color color_default;

    [Range(0.1f, 1f)]
    public float interval = 0.5f;
    [Range(5, 25)]
    public int times;

    [SerializeField]
    private BezierCurveTimer timer;

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        timer = new BezierCurveTimer();
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
        timer.Initialize(interval, times, this);
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
class BezierCurveTimer : Metronome
{
    private BezierCurveController controller;
    private IObjectManager objectManager;
    private float deltaT;
    [SerializeField]
    private float t;

    private VertexManager answer;
    private List<VertexManager> lines;
    [SerializeField]
    private List<Vector3> current;

    public void Initialize(float duration, float times, BezierCurveController controller)
    {
        base.Initialize(duration);
        objectManager = ServiceLocator.Get<IObjectManager>();
        this.controller = controller;
        answer = controller.GetComponentInChildren<VertexManager>();
        t = 0f;
        deltaT = 1 / times;
        current = new();
        current.AddRange(controller.vertexManager.Positions);
        lines = new();
        BezierCurve.CalculateNext(current, t);
    }

    protected override void MyOnComplete(float _)
    {
        if(current.Count > 1)
        {
            PaintLine();
            BezierCurve.CalculateNext(current, t);
        }
        else if(current.Count == 1)
        {
            PaintAnswer();
            current.Clear();
        }
        else
        {
            ClearLines();
            t += deltaT;
            current.Clear();
            current.AddRange(controller.vertexManager.Positions);
            BezierCurve.CalculateNext(current, t);
        }
        if(t < 1f + 1E-3)
            base.MyOnComplete(_);
    }

    private void PaintLine()
    {
        VertexManager vertexManager = objectManager.Activate("VertexManager", Vector3.zero, Vector3.zero, controller.transform).Transform.GetComponent<VertexManager>();
        for (int i = 0;i < current.Count;i++)
        {
            Vertex vertex = vertexManager.GenerateVertex(current[i]); 
            vertex.SetColor(controller.color_default);
        }
        lines.Add(vertexManager);
    }

    private void ClearLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].MyObject.Recycle();
        }
        lines.Clear();
    }

    private void PaintAnswer()
    {
        Vertex vertex = answer.GenerateVertex(current[0]);
        vertex.SetColor(controller.color_answer);
    }

    public void ClearAll()
    {
        ClearLines();
        answer.ClearVertices();
    }
}