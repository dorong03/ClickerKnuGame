using System;
using UnityEngine;

public enum StageType
{
    Home = 0,
    Lecture = 1,
    PartTime = 2,
    KidsCafe = 3,
    Library = 4,
    Restaurant = 5,
    CornerStore = 6
}

[Serializable]
public struct StageData
{
    public StageType stageType;
    public string stageName;
    public Sprite mainImage;

    public int knowledgeAdd;
    public int moneyAdd;
    public float staminaAdd;

    public AudioClip bgmClip;
    
    public GameObject targetButtonPanel;
}
