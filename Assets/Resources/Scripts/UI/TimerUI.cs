using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        timerText.text = $"{TimeManager.Instance.GetTimeText()}";
    }
    
    void OnEnable()
    {
        if (TimeManager.Instance != null)
        {
            timerText.text = $"{TimeManager.Instance.GetTimeText()}";
        }
        TimeManager.OnTimeChanged += UpdateTimerText;
    }

    void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateTimerText;
    }

    private void UpdateTimerText()
    {
        timerText.text = $"{TimeManager.Instance.GetTimeText()}";
    }
}
