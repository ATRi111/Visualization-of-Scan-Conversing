using Services;
using Services.Event;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

public class VertexManager : MonoBehaviour, ILine
{
    protected IObjectManager objectManager;
    protected IEventSystem eventSystem;

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
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    public virtual void GenerateVertex()
    {
        IMyObject obj = objectManager.Activate("Vertex", Vertex.MouseToWorld(0f), Vector3.zero, transform);
        vertices.Add(obj.Transform.GetComponent<Vertex>());
        dirty = true;
    }
    public void DeleteVertex(int index)
    {
        if (index >= 0 && index < vertices.Count)
        {
            vertices[index].GetComponent<MyObject>().Recycle();
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
        vertices.Clear();
        dirty = true;
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