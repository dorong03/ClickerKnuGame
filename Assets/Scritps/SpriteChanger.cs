using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;
    private int currentSprite;

    void Start()
    {
        if (sprites == null) return;
        if (image == null) return;
        currentSprite = 0;
        image.sprite = sprites[currentSprite];
    }

    public void OnSpriteChange()
    {
        currentSprite = (currentSprite + 1) % sprites.Length;
        image.sprite = sprites[currentSprite];
    }
}
