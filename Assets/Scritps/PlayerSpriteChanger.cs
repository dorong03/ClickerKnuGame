using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteChanger : MonoBehaviour
{
    [SerializeField] private Sprite basicSprite;
    [SerializeField] private Sprite secondGradeSprite;
    [SerializeField] private Sprite thirdGradeSprite;
    [SerializeField] private Sprite fourthGradeSprite;

    private Image playerImage;

    void Awake()
    {
        playerImage = GetComponent<Image>();
    }
    
    void OnEnable()
    {
        if (GameManager.Instance.CurrentSemester <= 2)
        {
            playerImage.sprite = basicSprite;
        }
        else if (GameManager.Instance.CurrentSemester <= 4)
        {
            playerImage.sprite = secondGradeSprite;
        }
        else if (GameManager.Instance.CurrentSemester <= 6)
        {
            playerImage.sprite = thirdGradeSprite;
        }
        else if (GameManager.Instance.CurrentSemester <= 8)
        {
            playerImage.sprite = fourthGradeSprite;
        }
    }
}
