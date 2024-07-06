using Tools;
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
        SetPositions(line.Positions);
    }

    public void SetPositions(Vector3[] positions)
    {
        if (positions == null || positions.Length == 0)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            Vector3[] copy = new Vector3[positions.Length];
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = positions[i].ResetZ(transform.position.z);
            }
            lineRenderer.positionCount = copy.Length;
            lineRenderer.SetPositions(copy);
            lineRenderer.enabled = true;
        }
    }
}