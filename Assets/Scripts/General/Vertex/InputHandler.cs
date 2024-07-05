using Services;
using Services.Event;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IEventSystem eventSystem;
    private DraggableVertexManager vertexManager;

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        vertexManager = GetComponentInParent<DraggableVertexManager>();
        eventSystem.AddListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.AddListener(EEvent.AfterReset, AfterReset);
    }

    private void OnDestroy()
    {
        eventSystem.RemoveListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.RemoveListener(EEvent.AfterReset, AfterReset);
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

    private void AfterLaunch()
    {
        enabled = false;
    }

    private void AfterReset()
    {
        enabled = true;
    }
}
