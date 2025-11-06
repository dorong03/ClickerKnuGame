using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum JobType
{
    None = 0,
    KidsCafe = 1,
    Library = 2,
    Restaurant = 3,
    CornerStore = 4
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGamePanelTitle;
    [SerializeField] private TextMeshProUGUI endGamePanelDescription;

    void Awake()
    {
        Instance = this;
        endGamePanel.SetActive(false);
    }
    
    public JobType jobType= JobType.None;
    
    void OnEnable()
    {
        TimeManager.OnSemesterChanged += InitPartTimeJob;
    }

    void OnDisable()
    {
        TimeManager.OnSemesterChanged -= InitPartTimeJob;
    }

    private void InitPartTimeJob()
    {
        jobType = JobType.None;
        if ((int)StageManager.Instance.CurrentStage.stageType > 2)
        {
            StageManager.Instance.ChangeStage(2);
        }
    }

    public void EndGame(string title, string description)
    {
        TimeManager.Instance.SetPause(true);
        endGamePanel.SetActive(true);
        endGamePanelTitle.text = title;
        endGamePanelDescription.text = description;
    }
    
    public void OnClickRetryButton()
    {
        Time.timeScale = 1f;
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name == null || obj.scene.name == "")
                Destroy(obj);
        }
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
