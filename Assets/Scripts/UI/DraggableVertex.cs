using Services;
using Services.Event;
using System;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableVertex : MonoBehaviour, IDragHandler
{
    private IEventSystem eventSystem;

    public static Vector3 MouseToWorld(float z)
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return temp.ResetZ(z);
    }

    public bool draggable;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!draggable)
            return;
        transform.position = MouseToWorld(transform.position.z);
        eventSystem.Invoke(EEvent.AfterVertexChange);
    }

    public void Align()
    {
        Vector3 v = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), v.z);
    }

    public float SqrDistanceToMouse()
    {
        return (transform.position - MouseToWorld(0f)).RemoveZ().sqrMagnitude;
    }
}
