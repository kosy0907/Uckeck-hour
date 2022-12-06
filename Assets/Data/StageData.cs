using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int level;
    public bool isClear;
    public int stars;
    public int clearCount;

    public StageData (GameManagerController manager)
    {
        level = manager.level;
        isClear = manager.isClear;
        stars = manager.stars;
        clearCount = manager.count;
    }
}
