using TMPro;
using UnityEngine;

public class Slider_Times : SliderBase<int>
{
    [SerializeField]
    private BezierCurveController controller;
    [SerializeField]
    private TextMeshProUGUI tmp;

    protected override void Awake()
    {
        base.Awake();
        Slider.value = DataToValue(controller.times);
    }

    protected override void OnValueChanged(float value)
    {
        int data = ValueToData(value);
        controller.times = data;
        tmp.text = $"绘制点个数:{data}";
    }

    protected override float DataToValue(int data)
    {
        return (data - 5f) / 20f;
    }

    protected override int ValueToData(float value)
    {
        return Mathf.RoundToInt(value * 20f) + 5;
    }
}
