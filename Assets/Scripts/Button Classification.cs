using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Box", order = 1)]
public class BoxClassification : ScriptableObject
{
    [SerializeField] public List<BoxClass> BoxTypeList;
}

public enum BoxType
{
    a, b, c
}

[System.Serializable]
public class BoxClass
{
    public BoxType type;
    public Sprite icon;
}
