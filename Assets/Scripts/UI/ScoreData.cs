using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData")]
public class ScoreData : ScriptableObject
{
    [SerializeField] public List<ScoreDataStruct> ScoreList;

    public ScoreDataStruct GetScore(int i)
    {
        return ScoreList[Mathf.Clamp(i, 0, ScoreList.Count - 1)];
    }
}

[System.Serializable]
public class ScoreDataStruct
{
    public int Score = 0;
    public string Name = "";
}