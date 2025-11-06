using System;
using TMPro;
using UnityEngine;

public class ExamUIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] subjectName;
    [SerializeField] private TextMeshProUGUI[] value;
    [SerializeField] private TextMeshProUGUI[] count;

    private void OnEnable()
    {
        UpdateExamButton();
        LectureManager.OnCourseProgressUpdated += UpdateExamButton;
    }

    private void OnDisable()
    {
        LectureManager.OnCourseProgressUpdated -= UpdateExamButton;
    }

    private void UpdateExamButton()
    {
        if (LectureManager.Instance == null) return;
        Debug.Log("시험 버튼 업데이트");
        int i = 0;
        foreach (var a in LectureManager.Instance.currentSemesterCourses)
        {
            subjectName[i].text = a.subject;
            value[i].text = $"소모\n지식\n1000";
            count[i].text = a.isExamCleared ? "1 / 1" : "0 / 1";
            i++;
        }
    }
}