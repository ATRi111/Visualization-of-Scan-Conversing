using Services;

public class Button_LaunchOrReset : ButtonBase
{
    private bool isRunning;
    public bool IsRunning
    {
        get => isRunning;
        set
        {
            if (isRunning != value)
            {
                isRunning = value;
                if(value)
                {
                    eventSystem.Invoke(EEvent.AfterLaunch);
                    tmp.text = "重置";
                }
                else
                {
                    eventSystem.Invoke(EEvent.AfterReset);
                    tmp.text = "启动";
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        isRunning = true;
        IsRunning = false;
    }

    protected override void OnClick()
    {
        IsRunning = !IsRunning;
    }
}
