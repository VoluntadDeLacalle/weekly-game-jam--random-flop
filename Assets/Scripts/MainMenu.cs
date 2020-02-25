using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    public bool canContinue;

    public TMPro.TextMeshProUGUI textMesh;

    void OnEnable()
    {
        canContinue = SaveSystem.LoadData() != null;

        if (!canContinue)
        {
            continueButton.SetActive(false);
            textMesh.text = "";
        }
        else
        {
            SaveAndExit data = SaveSystem.LoadData();
            Debug.Log(data.currentSavedLevel + " --- " + data.savedDifficulty);
            GameManager.instance.currentLevel = data.currentSavedLevel;
            GameManager.instance.highestLevel = data.highestSavedLevel;
            GameManager.instance.attempts = data.savedAttempts;
            GameManager.instance.difficulty = (GameManager.Difficulties)data.savedDifficulty;
            textMesh.text = "Level: " + GameManager.instance.currentLevel + "\n" + "Difficulty: " + GameManager.instance.difficulty.ToString();

            continueButton.SetActive(true);
        }
    }

}
