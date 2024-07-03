using Services;
using Services.ObjectPools;
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
    }

    protected virtual void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterDraggableVertexChange, AfterVertexChange);
    }
    protected virtual void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterDraggableVertexChange, AfterVertexChange);
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

    public override void GenerateVertex()
    {
        Vector3 p = Vertex.MouseToWorld(0f);
        if(!area.Contains(p))
            return;
        IMyObject obj = objectManager.Activate("DraggableVertex", p, Vector3.zero, transform);
        vertices.Add(obj.Transform.GetComponent<Vertex>());
        dirty = true;
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
}