using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void onStartSceneClicked()
    {
        SceneManager.LoadScene(1);
    }
}
