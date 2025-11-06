using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    void Start()
    {
        settingPanel.SetActive(false);
    }
    
    public void OnSettingOpenButton()
    {
        TimeManager.Instance.SetPause(true);
        settingPanel.SetActive(true);
    }
    
    public void OnSettingCloseButton()
    {
        TimeManager.Instance.SetPause(false);
        settingPanel.SetActive(false);
    }
}
