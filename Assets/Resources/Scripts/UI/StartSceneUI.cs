using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    public void OnClickTitleImage()
    {
        SceneManager.LoadScene(1);
    }
}
