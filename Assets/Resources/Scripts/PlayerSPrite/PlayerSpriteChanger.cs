using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteChanger : MonoBehaviour
{
    [SerializeField] private Sprite[] semesterSprites;
    [SerializeField] private Sprite[] partTimeSprites;

    [SerializeField] private Image playerImage;

    private void OnEnable()
    {
        TimeManager.OnSemesterChanged += UpdatePlayerSprite;
        StageManager.OnStageChange += OnStageChangeHandler;
        UpdatePlayerSprite();
    }

    private void OnDisable()
    {
        TimeManager.OnSemesterChanged -= UpdatePlayerSprite;
        StageManager.OnStageChange -= OnStageChangeHandler;
    }

    private void OnStageChangeHandler(StageData stage)
    {
        UpdatePlayerSprite();
    }

    private void UpdatePlayerSprite()
    {
        if (GameManager.Instance == null) return;
        if (playerImage == null) return;
        {
            if ((int)StageManager.Instance.CurrentStage.stageType > 2)
            {
                int jobIndex = (int)GameManager.Instance.jobType - 1;
                playerImage.sprite = partTimeSprites[jobIndex];
                return;
            }
            else
            {
                int semesterIndex = Mathf.Clamp((TimeManager.Instance.CurrentSemester - 1) / 2, 0, semesterSprites.Length - 1);
                playerImage.sprite = semesterSprites[semesterIndex];    
            }
        }
    }
}