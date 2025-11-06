using TMPro;
using UnityEngine;

public class StatusDisplayPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StaminaText;
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private TextMeshProUGUI KnowledgeText;

    void OnEnable()
    {
        StatManager.OnStatusChanged += UpdateStatusUI;
    }

    void OnDisable()
    {
        StatManager.OnStatusChanged -= UpdateStatusUI;
    }

    void Start()
    {
        UpdateStatusUI();
    }

    private void UpdateStatusUI()
    {
        if (StatManager.Instance == null) return;
        StaminaText.text = StatManager.Instance.GetStamina().ToString();
        MoneyText.text = StatManager.Instance.GetMoney().ToString();
        KnowledgeText.text = StatManager.Instance.GetKnowledge().ToString();
    }
}
