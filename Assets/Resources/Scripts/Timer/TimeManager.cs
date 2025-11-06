using System;
using UnityEngine;

public enum GameDayOfWeek 
{
    Monday = 0,
    Tuesday = 1,
    Wednesday = 2,
    Thursday = 3,
    Friday = 4,
    Saturday = 5,
    Sunday = 6
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public static event Action OnTimeChanged; 
    public static event Action OnSemesterChanged; 

    private const float SECONDS_PER_GAME_DAY = 15f; 
    
    [SerializeField] private float dayTimer = 0f;

    public bool IsPaused { get; private set; } = false;
    public int CurrentSemester { get; private set; } = 1;
    public int CurrentDayInSemester { get; private set; } = 1;
    public GameDayOfWeek CurrentDay { get; private set; } = GameDayOfWeek.Monday;

    public bool IsWeekend => CurrentDay == GameDayOfWeek.Saturday || CurrentDay == GameDayOfWeek.Sunday;

    private const int DAYS_PER_SEMESTER = 30;
    private const int MAX_SEMESTER = 8;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitTime();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (IsPaused)
        {
            return;
        }

        dayTimer += Time.deltaTime;
        if (dayTimer >= SECONDS_PER_GAME_DAY)
        {
            dayTimer -= SECONDS_PER_GAME_DAY;
            DayPass();
        }
    }
    
    private void InitTime()
    {
        CurrentSemester = 1;
        CurrentDayInSemester = 1;
        CurrentDay = GameDayOfWeek.Monday;
        dayTimer = 0f;
        IsPaused = false;
    }

    private void DayPass()
    {
        CurrentDayInSemester++;
        if (CurrentDayInSemester == 29)
        {
            NoticePrinter.Instance.SetNoticeText("시험이 곧 시작됩니다!\n과제를 모두 완료하세요.", 3, 1);
        } else if (CurrentDayInSemester == 27)
        {
            NoticePrinter.Instance.SetNoticeText("과제 마감 D-1!\n지금 바로 제출하세요", 3, 1);
        } else if (CurrentDayInSemester == 30)
        {
            NoticePrinter.Instance.SetNoticeText("등록금 납부 기간입니다.\n미납 시 퇴학을 당할 수도!", 3, 1);
        }
        
        CurrentDay = (GameDayOfWeek)(((int)CurrentDay + 1) % 7); 

        string notice = $"{CurrentSemester} 학기 {CurrentDayInSemester} 일, {GetDayOfWeekText(CurrentDay)}";
        
        if (CurrentDayInSemester > DAYS_PER_SEMESTER)
        {
            TuitionManager.Instance.CheckBeforeNextSemester();
            CurrentSemester++;
            CurrentDayInSemester = 1;
            
            OnSemesterChanged?.Invoke();
        }
        OnTimeChanged?.Invoke(); 
    }

    public void SetPause(bool pause)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1.0f;
    }

    public string GetDayOfWeekText(GameDayOfWeek day)
    {
        switch (day)
        {
            case GameDayOfWeek.Monday: return "월요일";
            case GameDayOfWeek.Tuesday: return "화요일";
            case GameDayOfWeek.Wednesday: return "수요일";
            case GameDayOfWeek.Thursday: return "목요일";
            case GameDayOfWeek.Friday: return "금요일";
            case GameDayOfWeek.Saturday: return "토요일";
            case GameDayOfWeek.Sunday: return "일요일";
            default: return "";
        }
    }

    public String GetTimeText()
    {
        if (Instance != null)
        {
            return $"{CurrentSemester} 학기 {CurrentDayInSemester} 일, {GetDayOfWeekText(CurrentDay)}";
        }

        return "";
    }
}