using System;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    public static event Action OnStatusChanged;

    [Header("스텟 초기값 설정")] 
    [SerializeField] private int knowledge = 0;
    [SerializeField] private int money = 0;
    [SerializeField] private float stamina = 100;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseStat(int knowledge, int money, float stamina)
    {
        this.knowledge += knowledge;
        this.money += money;
        this.stamina += stamina;
        
        this.knowledge = Mathf.Clamp(this.knowledge, 0, int.MaxValue);
        this.money = Mathf.Clamp(this.money, 0, int.MaxValue);
        this.stamina = Mathf.Clamp(this.stamina, 0f, 100f);

        if (this.stamina <= 30)
        {
            if (this.stamina <= 15)
            {
                NoticePrinter.Instance.SetNoticeText("너무 지쳤어요…\n쉬지 않으면 위험합니다.", 3, 1);   
            }
            else
            {
                NoticePrinter.Instance.SetNoticeText("체력이 부족합니다.\n방에서 쉬어야 겠어요.", 3, 1);    
            }
        } 
        
        OnStatusChanged?.Invoke();
        
        if (this.stamina <= 0)
        {
            GameManager.Instance.EndGame("번아웃 엔딩", "당신은 결국 버티지 못했습니다…");
        }
    }

    public int GetKnowledge()
    {
        return this.knowledge;
    }

    public int GetMoney()
    {
        return this.money;
    }

    public float GetStamina()
    {
        return this.stamina;
    }
}
