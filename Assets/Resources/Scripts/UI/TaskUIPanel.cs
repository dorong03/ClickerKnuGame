using System;
using TMPro;
using UnityEngine;

public class TaskUIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] taskName;
    [SerializeField] private TextMeshProUGUI[] value;
    [SerializeField] private TextMeshProUGUI[] count;

    private void OnEnable()
    {
        UpdateTaskButton();
        LectureManager.OnCourseProgressUpdated += UpdateTaskButton;
    }

    private void OnDisable()
    {
        LectureManager.OnCourseProgressUpdated -= UpdateTaskButton;
    }

    private void UpdateTaskButton()
    {
        if (LectureManager.Instance == null) return;
        Debug.Log("과제 버튼 업데이트");
        int i = 0;
        foreach (var a in LectureManager.Instance.currentSemesterCourses)
        {
            this.taskName[i].text = a.taskName;
            value[i].text = $"소모\n지식\n{a.CurrentAssignmentCost.ToString()}";
            count[i].text = $"{a.assignmentCount.ToString()} / 5";
            Debug.Log($"{a.taskName}\n소모\n지식\n{a.CurrentAssignmentCost.ToString()}\n{a.assignmentCount.ToString()} / 5");
            i++;
        }
    }
}
