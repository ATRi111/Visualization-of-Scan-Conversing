using EditorExtend;
using MyTimer;
using UnityEditor;

public class EdgeFlagTimerIndirectController : TimerIndirectEditor
{
    protected override string DefaultLabel => base.DefaultLabel;
    [AutoProperty]
    public SerializedProperty algorithm, discretedIndex;

    public EdgeFlagTimerIndirectController(SerializedProperty serializedProperty, ITimer timer, string label = null) : base(serializedProperty, timer, label)
    {

    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        EditorGUI.BeginDisabledGroup(true);
        discretedIndex.IntField("离散点索引");
        algorithm.PropertyField("算法过程");
        EditorGUI.EndDisabledGroup();
    }
}