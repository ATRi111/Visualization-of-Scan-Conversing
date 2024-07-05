using TMPro;
using UnityEngine;

public class Slider_Interval : SliderBase<float>
{
    [SerializeField]
    private BezierCurveController controller;
    [SerializeField]
    private TextMeshProUGUI tmp;

    protected override void Awake()
    {
        base.Awake();
        Slider.value = DataToValue(controller.interval);
    }

    protected override void OnValueChanged(float value)
    {
        float data = ValueToData(value);
        controller.interval = data;
        tmp.text = $"绘制间隔:{data:F2}秒";
    }
    protected override float DataToValue(float data)
    {
        return (data - 0.1f) / 0.9f;
    }
    protected override float ValueToData(float value)
    {
        return 0.9f * value + 0.1f;
    }
}
