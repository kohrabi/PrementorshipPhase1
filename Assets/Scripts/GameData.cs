using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int level;
    public bool isMuteMusic, isMuteSfx;
    public GameData()
    {

        Init();

    }
    public void Init()
    {
        level = 0;
        isMuteMusic = false;
        isMuteSfx = false;
    }
}
