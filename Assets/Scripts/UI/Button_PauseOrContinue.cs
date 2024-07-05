using Services;
using UnityEngine;

public class Button_PauseOrContinue : ButtonBase
{
    [SerializeField]
    private BezierCurveController controller;

    private bool paused;
    public bool Paused
    {
        get => paused;
        set
        {
            if (paused != value)
            {
                paused = value;
                if(value)
                    tmp.text = "继续";
                else
                    tmp.text = "暂停";
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Button.interactable = false;
        paused = true;
        Paused = false;
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.AddListener(EEvent.AfterReset, AfterReset);
    }
    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterLaunch, AfterLaunch);
        eventSystem.RemoveListener (EEvent.AfterReset, AfterReset);
    }

    private void AfterLaunch()
    {
        Button.interactable = true;
        Paused = false;
    }

    private void AfterReset()
    {
        Button.interactable = false;
    }

    protected override void OnClick()
    {
        Paused = !Paused;
        if (Paused)
            controller.Pause();
        else
            controller.Continue();
    }
}
