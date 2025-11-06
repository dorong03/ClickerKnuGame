using TMPro;
using UnityEngine;

public class InPartTimePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PartTimeName;
    [SerializeField] private TextMeshProUGUI PartTimeValue;
    [SerializeField] private TextMeshProUGUI TuitionFee;

    void Start()
    {
        InitPartTimeMoneyValue();
    }
    
    void OnEnable()
    {
        StatManager.OnStatusChanged += InitPartTimeMoneyValue;
        PartTimeName.text = StageManager.Instance.CurrentStage.stageName;
        PartTimeValue.text = $"클릭\n당 {StageManager.Instance.CurrentStage.knowledgeAdd}";
        InitPartTimeMoneyValue();
    }

    void OnDisable()
    {
        StatManager.OnStatusChanged -= InitPartTimeMoneyValue;
    }

    private void InitPartTimeMoneyValue()
    {
        if (TuitionManager.Instance == null) return;
        int fee = TuitionManager.Instance.isDeferred ? 4000000 : 2000000;
        TuitionFee.text = $"{StatManager.Instance.GetMoney().ToString("N0")} / {fee.ToString("N0")} 원";
    }
}
