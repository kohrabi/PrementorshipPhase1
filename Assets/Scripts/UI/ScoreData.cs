using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData")]
public class ScoreData : ScriptableObject
{
    [SerializeField] private List<ScoreDataStruct> ScoreList;

    public ScoreDataStruct GetScore(int i)
    {
        return ScoreList[Mathf.Clamp(i, 0, ScoreList.Count - 1)];
    }
}

[System.Serializable]
public class ScoreDataStruct
{
    public int Score;
    public string Name;
}