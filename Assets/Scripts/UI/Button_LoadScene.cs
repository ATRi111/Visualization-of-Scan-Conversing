using Services;
using Services.SceneManagement;
using UnityEngine;

public class Button_LoadScene : ButtonBase
{
    [SerializeField]
    private string sceneName;

    protected override void OnClick()
    {
        ServiceLocator.Get<ISceneController>().LoadScene(sceneName);
    }
}
