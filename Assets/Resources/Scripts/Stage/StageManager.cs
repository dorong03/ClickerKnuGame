using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }
    
    public static event Action<StageData> OnStageChange;
    
    [SerializeField] private StageData[] stageList;

    public StageData CurrentStage { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeStage(0);
    }

    public void ChangeStage(int stageIndex)
    {
        CurrentStage = stageList[stageIndex];
        Debug.Log($"맵 변경: {CurrentStage.stageType}");
        OnStageChange?.Invoke(CurrentStage);
    }

    public void OnClickPartTimeJob()
    {
        if (GameManager.Instance.jobType == JobType.None)
        {
            ChangeStage(2);
        }
        else
        {
            ChangeStage((int)GameManager.Instance.jobType + 2);
        }
    }

    public void OnClickPartTimeJobSelection(int jobIndex)
    {
        GameManager.Instance.jobType = (JobType)jobIndex;
        ChangeStage(jobIndex + 2);
    }
}
