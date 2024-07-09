using Services;
using Services.Event;
using UnityEngine.EventSystems;

public class DraggableVertex : Vertex, IDragHandler
{
    protected IEventSystem eventSystem;
    private Area2D area;
    public bool draggable;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        draggable = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (transform.parent != null)
            area = transform.parent.GetComponentInChildren<Area2D>();
        eventSystem.AddListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.AddListener(EEvent.AfterReset, AfterReset);
        Align();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.RemoveListener(EEvent.AfterReset, AfterReset);
    }

    private void AfterLaunch()
    {
        draggable = false;
    }

    private void AfterReset()
    {
        draggable = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!draggable)
            return;
        transform.position = MouseToWorld(transform.position.z);
        Align();
        if (area != null)
            transform.position = area.Restrict(transform.position);
        eventSystem.Invoke(EEvent.AfterDraggableVertexChange);
    }
}