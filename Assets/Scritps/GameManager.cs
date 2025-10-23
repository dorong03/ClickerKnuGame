using System;
using UnityEngine;

public enum PartTimeJobType
{
    None = 0,
    Library = 1,
    KidsCafe = 2,
    Restaurant = 3,
    CornerStore = 4
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public static event Action OnStatusChanged;
    public static event Action OnTimeChanged;
    public static event Action OnPartTimeChanged;
    public static event Action<string> OnNoticeUpdated;

    public const float SECONDS_PER_GAME_DAY = 20f;
    private float dayTimer ;
    private bool isPaused;

    private int currentSemester = 1;
    private int currentDayInSemester = 1;
    public DayOfWeek CurrentDay { get; private set; } = DayOfWeek.Monday;
    public bool IsWeekend => CurrentDay == DayOfWeek.Saturday || CurrentDay == DayOfWeek.Sunday;
    
    [SerializeField] private PartTimeJobType currentJob = PartTimeJobType.None;
    [SerializeField] private int knowledge;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private int money;

    public int Knowledge => knowledge;
    public float Stamina => stamina;
    public int Money => money;
    public int CurrentSemester => currentSemester;
    public int CurrentDayInSemester => currentDayInSemester;
    public PartTimeJobType CurrentJob => currentJob;
    public int PoorMoneyThreshold => poorMoneyThreshold; 
    
    [SerializeField] private int poorMoneyThreshold = 1300;

    private const int KNOWLEDGE_PER_CLICK = 10; 
        
    private const float STAMINA_RECOVERY_CLICK = 1.0f;
    private const float STAMINA_COST_PER_PART_CLICK = -0.5f;

    private const int MONEY_GAIN_LIBRARY = 100;
    private const int MONEY_GAIN_KIDS_CAFE = 200;
    private const int MONEY_GAIN_RESTAURANT = 300;
    private const int MONEY_GAIN_CONER_STORE= 400;

    void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void Start()
    {
        OnStatusChanged?.Invoke();
        OnTimeChanged?.Invoke();
        OnNoticeUpdated?.Invoke($"{CurrentSemester} 학기가 시작되었습니다.");
    }

    void Update()
    {
        if (isPaused)
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

    private void InitGame()
    {
        knowledge = 0;
        stamina = 100f; 
        money = 1000;
        currentJob = PartTimeJobType.None;
        
        currentSemester = 1;
        currentDayInSemester = 1;
        CurrentDay = DayOfWeek.Monday;
        dayTimer = 0f;
        isPaused = false;
    }

    private void DayPass()
    {
        currentDayInSemester++;
        CurrentDay = (DayOfWeek)(((int)CurrentDay + 1) % 7); 

        string notice = $"{CurrentSemester} 학기 {CurrentDayInSemester} 일, {GetDayOfWeekText(CurrentDay)}";
        
        if (CurrentDayInSemester > 30)
        {
            currentSemester++;
            currentDayInSemester = 1;
            currentJob = PartTimeJobType.None;
            
            notice = $"{CurrentSemester} 학기가 시작되었습니다.";

            if (CurrentSemester > 8)
            {
                EndGame("Graduation");
                return;
            }
        }
        
        OnTimeChanged?.Invoke();
        OnNoticeUpdated?.Invoke(notice);
        OnPartTimeChanged?.Invoke();
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1.0f;
    }
    

    public void AddKnowledge(int amount)
    {
        if (IsWeekend)
        {
            return;
        }
        
        knowledge += amount;
        OnStatusChanged?.Invoke();
    }

    public void ChangeStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina + amount, 0f, 100f);
        OnStatusChanged?.Invoke(); 

        if (stamina <= 0)
        {
            EndGame("BurnOut");
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        OnStatusChanged?.Invoke(); 
    }

    private void EndGame(string endingType)
    {
        SetPause(true);
        Debug.Log($"게임 종료: {endingType} 엔딩");
        OnNoticeUpdated?.Invoke($"게임 종료: {endingType} 엔딩");
    }

    public string GetDayOfWeekText(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return "월요일";
            case DayOfWeek.Tuesday: return "화요일";
            case DayOfWeek.Wednesday: return "수요일";
            case DayOfWeek.Thursday: return "목요일";
            case DayOfWeek.Friday: return "금요일";
            case DayOfWeek.Saturday: return "토요일";
            case DayOfWeek.Sunday: return "일요일";
            default: return "";
        }
    }
    
    public void OnClickMain()
    {
        ChangeStamina(STAMINA_RECOVERY_CLICK);
    }

    public void OnClickLecture()
    {
        AddKnowledge(KNOWLEDGE_PER_CLICK);
    }
    
    public void SelectPartTimeJob(int value)
    {
        currentJob = (PartTimeJobType)value;
        OnPartTimeChanged?.Invoke();
    }
    
    public void OnClickPartTime()
    {
        ChangeStamina(STAMINA_COST_PER_PART_CLICK);
        switch (currentJob)
        {
            case PartTimeJobType.None:
                Debug.LogError("알바가 선택되지 않았는데 ClickPartTime 실행됨");
                return;
            case PartTimeJobType.Library:
                AddMoney(MONEY_GAIN_LIBRARY);
                break;
            case PartTimeJobType.KidsCafe:
                AddMoney(MONEY_GAIN_KIDS_CAFE);
                break;
            case PartTimeJobType.Restaurant:
                AddMoney(MONEY_GAIN_RESTAURANT);
                break;
            case PartTimeJobType.CornerStore:
                AddMoney(MONEY_GAIN_CONER_STORE);
                break;
            default:
                break;
        }
    }
}

