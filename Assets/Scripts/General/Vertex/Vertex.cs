using Services.ObjectPools;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public static HashSet<Vertex> AllVertices = new();

    public static Vector3 MouseToWorld(float z)
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return temp.ResetZ(z);
    }

    public MyObject MyObject { get; protected set; }
    protected SpriteRenderer spriteRenderer;
    private Color color_default;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color_default = spriteRenderer.color;
        MyObject = GetComponent<MyObject>();
    }

    protected virtual void OnEnable()
    {
        AllVertices.Add(this);
    }

    protected virtual void OnDisable()
    {
        AllVertices.Remove(this);
        SetColor(color_default);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
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