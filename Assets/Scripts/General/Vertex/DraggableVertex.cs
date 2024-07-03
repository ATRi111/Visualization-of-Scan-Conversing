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

    private void OnEnable()
    {
        if (transform.parent != null)
            area = transform.parent.GetComponentInChildren<Area2D>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!draggable)
            return;
        transform.position = MouseToWorld(transform.position.z);
        if(area != null)
            transform.position = area.Restrict(transform.position);
        eventSystem.Invoke(EEvent.AfterDraggableVertexChange);
    }
}