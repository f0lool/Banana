using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public bool[] CompleteLevel;

    public GameData(LevelConfig[] levels)
    {
        CompleteLevel = new bool[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            CompleteLevel[i] = levels[i].Complete;
        }
    }
}
