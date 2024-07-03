using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private VertexManager vertexManager;

    private void Awake()
    {
        vertexManager = GetComponentInParent<VertexManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            vertexManager.GenerateVertex();
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            vertexManager.AlignVertex();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            vertexManager.DeleteVertex();
        }
    }
}
