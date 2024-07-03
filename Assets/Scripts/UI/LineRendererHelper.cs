using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    private ILine line;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        line = obj.GetComponent<ILine>();
    }

    private void Update()
    {
        Vector3[] vs = line.GetPositions();
        lineRenderer.positionCount = vs.Length;
        lineRenderer.SetPositions(vs);
    }
}
