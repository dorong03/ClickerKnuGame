using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingStatUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private float lifeTime = 0.6f;
    [SerializeField] private float moveUp = 40f;

    private float timer;
    private Vector3 startPos;
    private bool initialized = false;
    
    public void Init(Vector2 anchoredPos, Sprite sprite, string text)
    {
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;

        startPos = rt.localPosition;
        timer = lifeTime;
        initialized = true;

        if (icon != null) icon.sprite = sprite;
        if (amountText != null) amountText.text = text;
    }

    void Update()
    {
        if (!initialized) return;

        timer -= Time.deltaTime;
        float t = 1f - (timer / lifeTime);
        transform.localPosition = startPos + Vector3.up * (moveUp * t);

        if (timer <= 0f)
            Destroy(gameObject);
    }
}