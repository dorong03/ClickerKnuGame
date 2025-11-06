using UnityEngine;

public class LecturePanel : MonoBehaviour
{
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private GameObject examPanel;
    [SerializeField] private GameObject schedulePanel;

    void OnEnable()
    {
        taskPanel.SetActive(true);
        examPanel.SetActive(false);
        schedulePanel.SetActive(false);
    }

    public void OnClickTaskPanel()
    {
        taskPanel.SetActive(true);
        examPanel.SetActive(false);
        schedulePanel.SetActive(false);
    }

    public void OnClickExamPanel()
    {
        examPanel.SetActive(true);
        schedulePanel.SetActive(false);
        taskPanel.SetActive(false);
    }

    public void OnClickSchedulePanel()
    {
        schedulePanel.SetActive(true);
        examPanel.SetActive(false);
        taskPanel.SetActive(false);
    }
}
