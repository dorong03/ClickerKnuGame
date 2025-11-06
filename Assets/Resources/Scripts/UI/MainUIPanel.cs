using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUIPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image mainImage;

    [Header("Floating UI 설정")]
    [SerializeField] private FloatingStatUI floatingPrefab;
    [SerializeField] private RectTransform floatingParent;
    [SerializeField] private Sprite knowledgeIcon;

    private StageData currentStageData;

    void OnEnable()
    {
        StageManager.OnStageChange += OnStageChangedHandler;
    }

    void OnDisable()
    {
        StageManager.OnStageChange -= OnStageChangedHandler;
    }

    private void OnStageChangedHandler(StageData stageData)
    {
        currentStageData = stageData;
        if (mainImage != null)
            mainImage.sprite = stageData.mainImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentStageData.IsUnityNull()) return;

        if (currentStageData.stageType == StageType.Home && !TimeManager.Instance.IsWeekend)
            return;

        StatManager.Instance.IncreaseStat(
            currentStageData.knowledgeAdd,
            currentStageData.moneyAdd,
            currentStageData.staminaAdd
        );

        bool incKnowledge = currentStageData.knowledgeAdd > 0;
        bool incMoney = currentStageData.moneyAdd > 0;
        bool incStamina = currentStageData.staminaAdd > 0 && StatManager.Instance.GetStamina() < 100f;

        if (incKnowledge || incMoney || incStamina)
        {
            if (ClickComboManager.Instance != null)
                ClickComboManager.Instance.RegisterClick();
        }
        
        if (incKnowledge && floatingPrefab != null && floatingParent != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                floatingParent,
                eventData.position,
                null,
                out var localPos
            );

            var ui = Instantiate(floatingPrefab, floatingParent);
            ui.Init(localPos, knowledgeIcon, $"+{currentStageData.knowledgeAdd}");
        }
    }
}
