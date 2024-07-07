using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(XScanlineController))]
public class XScanlineControllerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty timer;
    public XScanlineTimerIndirectEditor timerEditor;

    protected override void OnEnable()
    {
        base.OnEnable();
        timerEditor = new XScanlineTimerIndirectEditor(timer, (target as XScanlineController).timer as XScanlineTimer, "计时器");    
    }

    protected override void MyOnInspectorGUI()
    {
        timerEditor.OnInspectorGUI();
    }
}