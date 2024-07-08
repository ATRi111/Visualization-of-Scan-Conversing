using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(EdgeFlagController))]
public class EdgeFlagControllerEditor : PaintControllerEditor
{
    [AutoProperty]
    public SerializedProperty timer;
    public EdgeFlagTimerIndirectController timerEditor;

    protected override void OnEnable()
    {
        base.OnEnable();
        timerEditor = new EdgeFlagTimerIndirectController(timer, (target as EdgeFlagController).timer, "¼ÆÊ±Æ÷");
    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        timerEditor.OnInspectorGUI();
    }
}