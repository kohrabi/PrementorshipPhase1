using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class Level_SO : ScriptableObject
{
    public List<Level_Info> lv_InfoList;
}

[System.Serializable]
public class Level_Info
{
    public int Level; //So thu tu cua level
    public int Time; //second
    public int MaxMove;
    public int RowSize;
    public int ColumnSize;
    public LevelUISettings UISettings;
}

[System.Serializable]
public class LevelUISettings
{
    public Vector2 CellSize = new Vector2(80f, 80f);
    public Vector2 SpacingSize = new Vector2(20f, 20f); 
}