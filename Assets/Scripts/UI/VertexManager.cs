using Services;
using Services.Event;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

public class VertexManager : MonoBehaviour ,ILine
{
    private IObjectManager objectManager;
    private IEventSystem eventSystem;

    private readonly List<DraggableVertex> vertices = new();

    [SerializeField]
    private float selectedDistance;
    [SerializeField]
    private int selectedIndex;
    [SerializeField]
    private Color color_selected;
    [SerializeField]
    private Color color_notSelected;

    private bool dirty;
    private Vector3[] vs;

    private void Awake()
    {
        objectManager = ServiceLocator.Get<IObjectManager>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        DraggableVertex[] temp = GetComponentsInChildren<DraggableVertex>();
        vertices.AddRange(temp);
        selectedIndex = -1;
        dirty = true;
        UpdatePositions();
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterVertexChange, AfterVertexChange);
    }
    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterVertexChange, AfterVertexChange);
    }


    private void Update()
    {
        UpdateIndex();
        UpdateColor();
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void AfterVertexChange()
    {
        dirty = true;
    }

    public void GenerateVertex()
    {
        IMyObject obj = objectManager.Activate("Vertex", DraggableVertex.MouseToWorld(0f), Vector3.zero, transform);
        vertices.Add(obj.Transform.GetComponent<DraggableVertex>());
        dirty = true;
    }
    public void DeleteVertex()
    {
        if(selectedIndex >= 0 && selectedIndex < vertices.Count)
        {
            vertices[selectedIndex].GetComponent<MyObject>().Recycle();
            vertices.RemoveAt(selectedIndex);
            dirty = true;
        }
    }
    public void AlignVertex()
    {
        if (selectedIndex >= 0 && selectedIndex < vertices.Count)
        {
            vertices[selectedIndex].Align();
            dirty = true;
        }
    }

    private void UpdatePositions()
    {
        if (dirty)
        {
            vs = new Vector3[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                vs[i] = vertices[i].transform.position;
            }
            dirty = false;
        }
    }
    public Vector3[] GetPositions()
    {
        UpdatePositions();
        return vs;
    }

    private void UpdateIndex()
    {
        selectedIndex = -1;
        if(vertices.Count > 0)
        {
            int minIndex = 0;
            for (int i = 1; i < vertices.Count; i++)
            {
                if (vertices[i].SqrDistanceToMouse() < vertices[minIndex].SqrDistanceToMouse())
                    minIndex = i;
            }
            if (vertices[minIndex].SqrDistanceToMouse() <= selectedDistance * selectedDistance)
                selectedIndex = minIndex;
        }
    }

    private void UpdateColor()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i].SetColor(i == selectedIndex ? color_selected : color_notSelected);
        }
    }
}