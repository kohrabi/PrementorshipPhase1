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
    a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r
}

[System.Serializable]
public class BoxClass
{
    public string Name;
    public BoxType type;
    public Sprite icon;
}
