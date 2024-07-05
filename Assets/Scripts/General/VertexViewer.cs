using Services;
using Services.ObjectPools;
using UnityEngine;

public class VertexViewer : MonoBehaviour
{
    private IObjectManager objectManager;

    [SerializeField]
    private Vertex selectedVertex;
    public Vertex SelectedVertex
    {
        get => selectedVertex;
        private set
        {
            if(selectedVertex != value)
            {
                if (mark != null)
                {
                    mark.Recycle();
                    mark = null;
                }
                if(value != null && value is not DraggableVertex)
                {
                    mark = objectManager.Activate("Mark", value.transform.position, Vector3.zero, transform);
                }
                selectedVertex = value;
            }
        }
    }

    [SerializeField]
    private float selectedDistance;

    private IMyObject mark;

    private void Awake()
    {
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void OnEnable()
    {
        selectedVertex = null;
    }

    private void Update()
    {
        UpdateSelected();
    }

    private void UpdateSelected()
    {
        Vertex min = null;
        foreach (var vertex in Vertex.AllVertices)
        {
            if (min == null || vertex.SqrDistanceToMouse() < min.SqrDistanceToMouse())
                min = vertex;
        }
        if (min != null && min.SqrDistanceToMouse() <= selectedDistance * selectedDistance)
            SelectedVertex = min;
        else
            SelectedVertex = null;
    }
}
