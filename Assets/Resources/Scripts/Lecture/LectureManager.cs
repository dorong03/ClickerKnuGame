using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LectureManager : MonoBehaviour
{
    public static LectureManager Instance { get; private set; }
    public static event Action OnCourseProgressUpdated;

    private const int ASSIGNMENT_MAX = 5;
    private const int EXAM_COST = 1000;

    private List<CourseData> allCourses = new List<CourseData>();
    public List<CourseData> currentSemesterCourses { get; private set; } = new List<CourseData>();

    [SerializeField] private string csvResourcePath = "CourseTable";

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        TimeManager.OnSemesterChanged += LoadCoursesForCurrentSemester;
    }

    private void OnDisable()
    {
        TimeManager.OnSemesterChanged -= LoadCoursesForCurrentSemester;
    }

    private void Start()
    {
        LoadAllCoursesFromCsv();
        LoadCoursesForCurrentSemester();
    }

    private void LoadAllCoursesFromCsv()
    {
        allCourses = CSVLoader.LoadFromResources(csvResourcePath);
        Debug.Log($"[LectureManager] CSV에서 전체 과목 {allCourses.Count}개 로드.");
    }

    private void LoadCoursesForCurrentSemester()
    {
        int currentSemester = TimeManager.Instance.CurrentSemester;
        currentSemesterCourses = allCourses.Where(c => c.semester == currentSemester).ToList();
        Debug.Log($"{currentSemester}학기 과목 {currentSemesterCourses.Count}개 로드 완료.");
        OnCourseProgressUpdated?.Invoke();
    }

    public bool CompleteAssignment(int courseId)
    {
        courseId = ((TimeManager.Instance.CurrentSemester-1) * 4) + courseId;
        CourseData course = currentSemesterCourses.FirstOrDefault(c => c.id == courseId);
        if (course == null)
        {
            Debug.Log("존재하지 않는 과제ID");
            return false;
        }

        if (TimeManager.Instance.CurrentDayInSemester >= 29)
        {
            NoticePrinter.Instance.SetNoticeText("과제 제출일이 마감되었습니다.", 3, 1);
        }
        if (course.assignmentCount >= ASSIGNMENT_MAX)
        {
            Debug.Log("이미 최대치입니다.");
            return false;
        }

        if (StatManager.Instance.GetKnowledge() < course.CurrentAssignmentCost)
        {
            Debug.Log("지식이 부족합니다.");
            NoticePrinter.Instance.SetNoticeText("지식이 부족해 과제를 완료할 수 없습니다!", 3, 1);
            return false;
        }

        StatManager.Instance.IncreaseStat(-course.CurrentAssignmentCost, 0, 0);
        course.assignmentCount++;
        OnCourseProgressUpdated?.Invoke();
        Debug.Log($"{course.subject} 과제 완료. 남은 지식: {StatManager.Instance.GetKnowledge()}");
        return true;
    }

    public bool TakeExam(int courseId)
    {
        courseId = ((TimeManager.Instance.CurrentSemester-1) * 4) + courseId;
        CourseData course = currentSemesterCourses.FirstOrDefault(c => c.id == courseId);
        if (course == null) return false;
        if (TimeManager.Instance.CurrentDayInSemester < 29)
        {
            NoticePrinter.Instance.SetNoticeText("시험 기간이 아닙니다.", 3, 1);
        }
        if (course.isExamCleared)
        {
            NoticePrinter.Instance.SetNoticeText("이미 완료한 시험입니다.", 3, 1);
            return false;
        }
        if (course.assignmentCount < ASSIGNMENT_MAX)
        {
            NoticePrinter.Instance.SetNoticeText("과제를 완료한 후 시험을 진행할 수 있습니다.", 3, 1);
            return false;
        }
        if (StatManager.Instance.GetKnowledge() < EXAM_COST)
        {
            NoticePrinter.Instance.SetNoticeText("지식이 부족해 시험을 볼 수 없습니다!", 3, 1);
            return false;
        }

        StatManager.Instance.IncreaseStat(-EXAM_COST, 0, 0);
        course.isExamCleared = true;
        OnCourseProgressUpdated?.Invoke();
        Debug.Log($"{course.subject} 시험 완료.");
        return true;
    }

    public void OnClickExamButton(int index)
    {
        TakeExam(index);
    }

    public void OnClickTaskButoon(int index)
    {
        CompleteAssignment(index);
        Debug.Log(index);
    }
}
