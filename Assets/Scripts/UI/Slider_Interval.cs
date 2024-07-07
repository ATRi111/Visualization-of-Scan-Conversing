using TMPro;
using UnityEngine;

public class Slider_Interval : SliderBase<float>
{
    [SerializeField]
    private GameObject obj_controller;
    private IPaintController controller;
    [SerializeField]
    private TextMeshProUGUI tmp;

    protected override void Awake()
    {
        base.Awake();
        controller = obj_controller.GetComponent<IPaintController>();
    }

    private void Start()
    {
        Slider.value = DataToValue(controller.Interval);
    }

    protected override void OnValueChanged(float value)
    {
        float data = ValueToData(value);
        controller.Interval = data;
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
