using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float magnitude = 8f;

    private Vector3 originalPos;
    private float timer;
    private bool isShaking = false;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (!isShaking) return;

        timer -= Time.deltaTime;
        if (timer > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = originalPos + new Vector3(x, y, 0f);
        }
        else
        {
            isShaking = false;
            transform.localPosition = originalPos;
        }
    }

    public void Shake()
    {
        timer = duration;
        isShaking = true;
    }
}