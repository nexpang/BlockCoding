using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public StageInfo[] stageInfos;
}

[System.Serializable]
public class StageInfo
{
    public string stageName;
    public Sprite stageSprite;
}