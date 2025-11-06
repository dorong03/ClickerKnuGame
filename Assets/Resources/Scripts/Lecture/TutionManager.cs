using System.Linq;
using UnityEngine;

public class TuitionManager : MonoBehaviour
{
    public static TuitionManager Instance { get; private set; }

    public const int TUITION_FEE = 2000000;
    private const int MAX_SEMESTER = 8;

    private bool currentSemesterPaid = false;
    public bool isDeferred { get; private set; } = false;
    private int totalDeferredCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void TryPayTuition()
    {
        if (currentSemesterPaid)
        {
            NoticePrinter.Instance.SetNoticeText("이미 등록금을 전부 납부했습니다.", 3, 1);
            return;
        }
        if (StatManager.Instance.GetMoney() < (isDeferred ? TUITION_FEE * 2 : TUITION_FEE))
        {
            NoticePrinter.Instance.SetNoticeText("돈이 부족합니다!\n등록금을 낼 수 없어요.", 3, 1);
            return;
        }
        
        int payAmount = isDeferred ? TUITION_FEE * 2 : TUITION_FEE;
        StatManager.Instance.IncreaseStat(0, -payAmount, 0);
        currentSemesterPaid = true;
        NoticePrinter.Instance.SetNoticeText("등록금을 납부했습니다!", 3, 1);
        isDeferred = false;
    }
    
    public void CheckBeforeNextSemester()
    {
        int currentSemester = TimeManager.Instance.CurrentSemester;
        var semesterCourses = LectureManager.Instance.currentSemesterCourses;

        bool allCleared = semesterCourses.All(c => c.isExamCleared);
        if (!allCleared)
        {
            GameManager.Instance.EndGame("시험 미응시", $"{currentSemester}학기 과목을 모두 이수하지 못했습니다.");
            return;
        }

        if (!currentSemesterPaid)
        {
            totalDeferredCount++;
            if (totalDeferredCount >= 2)
            {
                GameManager.Instance.EndGame("빚더미 엔딩", "등록금을 2회 이상 못 냈습니다.\n당신의 대학생활은 끝났습니다.");
                return;
            }

            isDeferred = true;
            Debug.Log("등록금 이월됨");
        }
        
        currentSemesterPaid = false;
        if (currentSemester >= MAX_SEMESTER)
        {
            if (totalDeferredCount == 0)
            {
                GameManager.Instance.EndGame("졸업 엔딩", "드디어 졸업입니다! \n강의와 과제,시험을 열심히 치룬 당신이 대단합니다");
            }
            else
            {
                GameManager.Instance.EndGame("빚더미 엔딩","등록금을 못 냈습니다.\n당신의 대학생활은 끝났습니다.");
            }
        }
    }
}
