using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveAndExit
{
    public int currentSavedLevel;
    public int savedDifficulty;
    public int highestSavedLevel;
    public int savedAttempts;

    public SaveAndExit()
    {
        currentSavedLevel = GameManager.instance.currentLevel;
        savedDifficulty = (int)GameManager.instance.difficulty;
        highestSavedLevel = GameManager.instance.highestLevel;
        savedAttempts = GameManager.instance.attempts;
    }

}
