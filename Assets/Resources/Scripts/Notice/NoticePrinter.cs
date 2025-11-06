using System.Collections;
using TMPro;
using UnityEngine;

public class NoticePrinter : MonoBehaviour
{
    public static NoticePrinter Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI noticeText;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        if (noticeText != null)
            noticeText.alpha = 0f;
    }

    public void SetNoticeText(string text, float duration, float fadeTime)
    {
        if (noticeText == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowNotice(text, duration, fadeTime));
    }

    private IEnumerator ShowNotice(string text, float duration, float fadeTime)
    {
        noticeText.text = text;
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeTime);
            noticeText.alpha = alpha;
            yield return null;
        }
        noticeText.alpha = 1f;
        yield return new WaitForSeconds(duration);
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            noticeText.alpha = alpha;
            yield return null;
        }
        noticeText.alpha = 0f;

        currentRoutine = null;
    }
}