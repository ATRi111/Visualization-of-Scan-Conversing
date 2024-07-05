using UnityEngine;

public class Grid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Color Color
    {
        get => spriteRenderer.color;
        set
        {
            spriteRenderer.color = value;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        Color = Color.white;
    }
}
