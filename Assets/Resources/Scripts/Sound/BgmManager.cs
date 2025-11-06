using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance { get; private set; }

    [Header("BGM 출력용 오디오소스")]
    [SerializeField] private AudioSource bgmSource;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        StageManager.OnStageChange += OnStageChanged;
    }

    void OnDisable()
    {
        StageManager.OnStageChange -= OnStageChanged;
    }

    private void OnStageChanged(StageData stage)
    {
        PlayBgm(stage.bgmClip);
    }

    private void PlayBgm(AudioClip clip)
    {
        if (bgmSource == null) return;

        if (clip == null)
        {
            bgmSource.Stop();
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
