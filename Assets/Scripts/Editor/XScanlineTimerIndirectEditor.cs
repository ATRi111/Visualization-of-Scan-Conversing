using EditorExtend;
using MyTimer;
using UnityEditor;

public class XScanlineTimerIndirectEditor : TimerIndirectEditor
{
    protected override string DefaultLabel => base.DefaultLabel;
    [AutoProperty]
    public SerializedProperty algorithm, state;
    
    public XScanlineTimerIndirectEditor(SerializedProperty serializedProperty, ITimer timer, string label = null) : base(serializedProperty, timer, label)
    {

    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        EditorGUI.BeginDisabledGroup(true);
        algorithm.PropertyField("算法过程");
        state.EnumField<XScanlineTimer.EState>("当前步骤");
        EditorGUI.EndDisabledGroup();
    }
}