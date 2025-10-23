using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI knowledgeText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI noticeText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image mainPanelImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite poorSprite;
    [SerializeField] private GameObject mainDefaultButton;
    [SerializeField] private GameObject mainPartTimeButton;
    
    [SerializeField] private GameObject lecturePanel;
    [SerializeField] private GameObject partTimeJobPanel;
    [SerializeField] private GameObject settingPanel;

    [SerializeField] private GameObject libraryPanel;
    [SerializeField] private GameObject kidsCafePanel;
    [SerializeField] private GameObject restaurantPanel;
    [SerializeField] private GameObject cornerStorePanel;
    
    [SerializeField] private float noticeDisplayTime = 2.0f;
    [SerializeField] private float noticeFadeTime = 1.0f;
    private Coroutine fadeNoticeCoroutine;

    void OnEnable()
    {
        GameManager.OnStatusChanged += UpdateStatusUI;
        GameManager.OnTimeChanged += UpdateTimeUI;
        GameManager.OnNoticeUpdated += UpdateNoticeUI;
        GameManager.OnPartTimeChanged += UpdatePartTimeUI;
    }
    
    private void OnDisable()
    {
        GameManager.OnStatusChanged -= UpdateStatusUI;
        GameManager.OnTimeChanged -= UpdateTimeUI;
        GameManager.OnNoticeUpdated -= UpdateNoticeUI;
        GameManager.OnPartTimeChanged -= UpdatePartTimeUI;
    }

    void Start()
    {
        ShowPanel(mainPanel);
        UpdateStatusUI();
        UpdateTimeUI();
        if(noticeText != null)
        {
            SetNoticeAlpha(0f);
        }
    }

    public void OnClickLectureButton()
    {
        ShowPanel(lecturePanel);
    }
    
    public void OnClickPartTimeJobButton()
    {
        if (GameManager.Instance.CurrentJob == PartTimeJobType.None)
        {
            mainDefaultButton.SetActive(false);
            mainPartTimeButton.SetActive(true);
            Debug.Log("2");
        }
        else
        {
            Debug.Log("3");
            ShowPanel(partTimeJobPanel);
            libraryPanel.SetActive(GameManager.Instance.CurrentJob == PartTimeJobType.Library);
            kidsCafePanel.SetActive(GameManager.Instance.CurrentJob == PartTimeJobType.KidsCafe);
            restaurantPanel.SetActive(GameManager.Instance.CurrentJob == PartTimeJobType.Restaurant);
            cornerStorePanel.SetActive(GameManager.Instance.CurrentJob == PartTimeJobType.CornerStore);
        }
    }

    public void OnClickHomeButton()
    {
        ShowPanel(mainPanel);
    }

    public void OnClickSettingButton()
    {
        settingPanel.SetActive(true);
        GameManager.Instance.SetPause(true);
    }

    public void OnClickCloseSettingButton()
    {
        settingPanel.SetActive(false);
        GameManager.Instance.SetPause(false);
    }

    private void ShowPanel(GameObject targetPanel)
    {
        mainDefaultButton.SetActive(true);
        mainPartTimeButton.SetActive(false);
        mainPanel.SetActive(targetPanel == mainPanel);
        lecturePanel.SetActive(targetPanel == lecturePanel);
        partTimeJobPanel.SetActive(targetPanel == partTimeJobPanel);
    }

    public void UpdatePartTimeUI()
    {
        OnClickPartTimeJobButton();
    }
    
    private void UpdateStatusUI()
    {
        knowledgeText.text = GameManager.Instance.Knowledge.ToString("N0");
        moneyText.text = GameManager.Instance.Money.ToString("N0") + " 원";
        staminaText.text = $"{GameManager.Instance.Stamina:F1} %";
        if (GameManager.Instance.Money < GameManager.Instance.PoorMoneyThreshold)
        {
            Debug.Log("poor");
            mainPanelImage.sprite = poorSprite;
        }
        else
        {
            Debug.Log("normal");
            mainPanelImage.sprite = normalSprite;
        }
    }

    private void UpdateTimeUI()
    {
        if (timeText == null) return;
        timeText.text =
            $"{GameManager.Instance.CurrentSemester} 학기 {GameManager.Instance.CurrentDayInSemester} 일 {GameManager.Instance.GetDayOfWeekText(GameManager.Instance.CurrentDay)}";
    }

    private void UpdateNoticeUI(string notice)
    {
        if (noticeText == null) return;
        
        if (fadeNoticeCoroutine != null)
        {
            StopCoroutine(fadeNoticeCoroutine);
        }
        fadeNoticeCoroutine = StartCoroutine(FadeNoticeRoutine(notice));
    }

    private IEnumerator FadeNoticeRoutine(string notice)
    {
        noticeText.text = notice;
        SetNoticeAlpha(1f);
        
        yield return new WaitForSeconds(noticeDisplayTime);

        float timer = 0f;
        while (timer < noticeFadeTime)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / noticeFadeTime);
            SetNoticeAlpha(alpha);
            yield return null;
        }
        
        SetNoticeAlpha(0f);
    }

    private void SetNoticeAlpha(float alpha)
    {
        Color color = noticeText.color;
        color.a = alpha;
        noticeText.color = color;
    }
}

