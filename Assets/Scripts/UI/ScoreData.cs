using System.Collections.Generic;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName ="ScriptableObjects/ScoreData")]
public class ScoreDatas : ScriptableObject
{
	[SerializeField] public List<ScoreData> ScoreList = new List<ScoreData>();

	public ScoreData GetScore(int i)
	{
		return ScoreList[Mathf.Clamp(i, 0, ScoreList.Count - 1)];
	}
}

[System.Serializable]
public class ScoreData
{
	public int Score = 0;
	public string Name = "";
}