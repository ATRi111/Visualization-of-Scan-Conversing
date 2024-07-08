using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(XScanlineController))]
public class XScanlineControllerEditor : PaintControllerEditor
{
    [AutoProperty]
    public SerializedProperty timer;
    public XScanlineTimerIndirectEditor timerEditor;

    protected override void OnEnable()
    {
        base.OnEnable();
        timerEditor = new XScanlineTimerIndirectEditor(timer, (target as XScanlineController).timer, "计时器");    
    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        timerEditor.OnInspectorGUI();
    }
}