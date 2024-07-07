using EditorExtend;
using UnityEditor;

public class PaintControllerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty vertexManager, interval_init;

    protected override void MyOnInspectorGUI()
    {
        vertexManager.PropertyField("VertexManager");
        interval_init.FloatField("预设绘制间隔");
    }
}