using UnityEngine;

public class Text_SelectedVertex : TextBase
{
    [SerializeField]
    private VertexViewer viewer;

    private void Update()
    {
        if (viewer.SelectedVertex == null)
            TextUI.text = "(无)";
        else
        {
            Vector3 v = viewer.SelectedVertex.transform.position;
            TextUI.text = $"x={v.x:0.00;-0.00}\n" +
                $"y={v.y:0.00;-0.00}";
        }
    }
}
