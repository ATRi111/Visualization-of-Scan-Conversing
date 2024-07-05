using Services;
using UnityEngine;

public class DraggableVertexManager : VertexManager
{
    protected Area2D area;

    [SerializeField]
    private float selectedDistance;
    [SerializeField]
    private int selectedIndex;
    [SerializeField]
    private Color color_selected;
    [SerializeField] 
    private Color color_notSelected;

    protected override void Awake()
    {
        base.Awake();
        area = GetComponentInChildren<Area2D>();
        DraggableVertex[] temp = GetComponentsInChildren<DraggableVertex>();
        vertices.AddRange(temp);
        selectedIndex = -1;

        eventSystem.AddListener(EEvent.AfterDraggableVertexChange, AfterVertexChange);
        eventSystem.AddListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.AddListener(EEvent.AfterReset, AfterReset);
    }

    protected void OnDestroy()
    {
        eventSystem.RemoveListener(EEvent.AfterDraggableVertexChange, AfterVertexChange);
        eventSystem.RemoveListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.RemoveListener(EEvent.AfterReset, AfterReset);
    }

    protected override void Update()
    {
        base.Update();
        UpdateIndex();
        UpdateColor();
    }

    private void AfterVertexChange()
    {
        dirty = true;
    }

    public Vertex GenerateVertex()
    {
        Vector3 p = Vertex.MouseToWorld(0f);
        if (!area.Contains(p))
            return null;

        Vertex vertex = objectManager.Activate("DraggableVertex", p, Vector3.zero, transform).Transform.GetComponent<Vertex>();
        vertices.Add(vertex);
        dirty = true;
        return vertex;
    }
    public void DeleteVertex()
    {
        DeleteVertex(selectedIndex);
    }
    public void AlignVertex()
    {
        if (selectedIndex >= 0 && selectedIndex < vertices.Count)
        {
            vertices[selectedIndex].Align();
            dirty = true;
        }
    }

    protected void UpdateIndex()
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

    protected void UpdateColor()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i].SetColor(i == selectedIndex ? color_selected : color_notSelected);
        }
    }

    private void AfterLaunch()
    {
        selectedIndex = 1;
        enabled = false;
    }

    private void AfterReset()
    {
        enabled = true;
    }
}