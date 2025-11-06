using UnityEngine;

public class ButtonUIPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] buttonPanelList;
    
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
        foreach (GameObject panel in buttonPanelList)
        {
            bool isActive = (panel == stageData.targetButtonPanel);
            panel.SetActive(isActive);
        }
    }
}
