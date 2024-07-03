using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private DraggableVertexManager vertexManager;

    private void Awake()
    {
        vertexManager = GetComponentInParent<DraggableVertexManager>();
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
