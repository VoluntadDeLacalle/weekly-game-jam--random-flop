using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    public bool canContinue;

    public TMPro.TextMeshProUGUI textMesh;
    public TMPro.TextMeshProUGUI titleMesh;
    Sequence colorSequence;

    public bool showColor;

    public float colorChangeTime;

    void Awake()
    {
        showColor = false;
    }

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

    void Update()
    {
        if (!showColor)
        {
            colorText(textMesh);
            colorText(titleMesh);
            showColor = true;
        }
    }

    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    void colorText(TMPro.TextMeshProUGUI txt)
    {
        txt.color = GameManager.instance.colorList[5];

        colorSequence = DOTween.Sequence();
        colorSequence.Append(txt.DOColor(GameManager.instance.colorList[0], colorChangeTime))
            .AppendCallback(() => { txt.color = GameManager.instance.colorList[0]; })
                    .Append(txt.DOColor(GameManager.instance.colorList[1], colorChangeTime))
            .AppendCallback(() => { txt.color = GameManager.instance.colorList[1]; })
                    .Append(txt.DOColor(GameManager.instance.colorList[2], colorChangeTime))
            .AppendCallback(() => { txt.color = GameManager.instance.colorList[2]; })
                    .Append(txt.DOColor(GameManager.instance.colorList[3], colorChangeTime))
            .AppendCallback(() => { txt.color = GameManager.instance.colorList[3]; })
                    .Append(txt.DOColor(GameManager.instance.colorList[4], colorChangeTime))
            .AppendCallback(() => { txt.color = GameManager.instance.colorList[4]; })
                    .Append(txt.DOColor(GameManager.instance.colorList[5], colorChangeTime))
            .AppendCallback(() => { txt.color = GameManager.instance.colorList[5]; }).SetLoops(-1);
    }

}
