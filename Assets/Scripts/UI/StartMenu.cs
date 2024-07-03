using Services;
using Services.SceneManagement;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private ISceneController sceneController;

    private void Awake()
    {
        sceneController = ServiceLocator.Get<ISceneController>();
        sceneController.LoadScene("BezierCurve");
    }
}
