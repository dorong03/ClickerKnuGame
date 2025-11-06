using UnityEngine;

public class ClickComboManager : MonoBehaviour
{
    public static ClickComboManager Instance { get; private set; }

    [SerializeField] private float comboInterval = 0.5f;
    [SerializeField] private int shakeComboThreshold = 30;
    [SerializeField] private ScreenShaker screenShaker;

    private float lastClickTime = -999f;
    public int currentCombo { get; private set; } = 0;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterClick()
    {
        float now = Time.time;

        if (now - lastClickTime <= comboInterval)
        {
            currentCombo++;
        }
        else
        {
            currentCombo = 1;
        }

        lastClickTime = now;

        if (currentCombo >= shakeComboThreshold && screenShaker != null)
        {
            screenShaker.Shake();
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        lastClickTime = -999f;
    }
}