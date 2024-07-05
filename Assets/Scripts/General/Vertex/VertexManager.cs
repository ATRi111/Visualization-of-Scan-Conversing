using Services;
using Services.Event;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

public class VertexManager : MonoBehaviour, ILine
{
    protected IObjectManager objectManager;
    protected IEventSystem eventSystem;
    public MyObject MyObject { get ; protected set; }
    protected readonly List<Vertex> vertices = new();

    public bool dirty;
    protected Vector3[] positions;

    public Vector3[] Positions
    {
        get
        {
            if (dirty)
            {
                UpdatePositions();
                dirty = false;
            }
            return positions;
        }
    }

    protected virtual void Awake()
    {
        objectManager = ServiceLocator.Get<IObjectManager>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        dirty = true;
        MyObject = GetComponent<MyObject>();
        MyObject.OnActivate += OnActivate;
        MyObject.OnRecycle += OnRecycle;
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnActivate()
    {
        dirty = true;
    }

    protected virtual void OnRecycle()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        vertices.Clear();
    }

    public Vertex GenerateVertex(Vector3 position)
    {
        Vertex vertex = objectManager.Activate("Vertex", position, Vector3.zero, transform).Transform.GetComponent<Vertex>();
        vertices.Add(vertex);
        dirty = true;
        return vertex;
    }
    public void DeleteVertex(int index)
    {
        if (index >= 0 && index < vertices.Count)
        {
            vertices[index].MyObject.Recycle();
            vertices.RemoveAt(index);
            dirty = true;
        }
    }
    public void AlignVertex(int index)
    {
        if (index >= 0 && index < vertices.Count)
        {
            vertices[index].Align();
            dirty = true;
        }
    }
    public void ClearVertices()
    {
        if(vertices.Count > 0)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].MyObject.Recycle();
            }
            vertices.Clear();
            dirty = true;
        }
    }

    protected void UpdatePositions()
    {
        positions = new Vector3[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            positions[i] = vertices[i].transform.position;
        }
        dirty = false;
    }
}