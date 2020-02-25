using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public TMPro.TextMeshProUGUI levelTextMesh;
    public TMPro.TextMeshProUGUI tempFlavorTextMesh;
    public TMPro.TextMeshProUGUI highestLevelMesh;

    public int maxLevels;
    public int currentLevel;
    public int highestLevel;
    public int attempts;

    public bool gameStart;
    public bool hasWon;

    public enum Difficulties
    {
        Easy,
        Medium,
        Hard,
        Insane,
        Impossible
    }

    public Dictionary<Difficulties, float> difficultyValues =
        new Dictionary<Difficulties, float>
        {   {Difficulties.Easy, 0.75f},
            {Difficulties.Medium, 0.5f},
            {Difficulties.Hard, 0.35f},
            {Difficulties.Insane, 0.2f},
            {Difficulties.Impossible, 0.1f}
        };

    public Difficulties difficulty;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        currentLevel = 1;
        highestLevel = 1;
        attempts = 0;
        hasWon = false;
        gameStart = false;
    }

    void Start()
    {
        levelTextMesh.text = "Level: " + currentLevel + "\nDifficulty: " + difficulty.ToString() + "\nAttempts: " + attempts;
        highestLevelMesh.text = "Highest Level: " + highestLevel;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && gameStart && !hasWon)
        {
            tempFlavorTextMesh.transform.DOKill();
            tempFlavorTextMesh.transform.DOScale(new Vector3(0.0001f, 0.0001f, 0.0001f), 0.25f);
            tryNextLevel();
        }
    }

    public void startGame(int diffIndex)
    {
        gameStart = true;
        difficulty = (Difficulties)diffIndex;
        Debug.Log("Your current difficulty value is: " + difficultyValues[difficulty]);
    }

    void tryNextLevel()
    {
        float rand = Random.Range(0.0f, 1.0f);

        if (rand <= difficultyValues[difficulty])
        {
            //Debug.Log("Next Level!");
            currentLevel++;

            if (currentLevel > maxLevels)
            {
                levelTextMesh.text = "You win!! ...Somehow?";
                hasWon = true;

                flavorText("Winner!");

                return;
            }

            if (currentLevel > highestLevel)
            {
                highestLevel = currentLevel;
            }

            flavorText("Next Level!");
        }
        else
        {
            //Debug.Log("Flop!");
            currentLevel = 1;

            flavorText("Flop!");
        }

        attempts++;

        levelTextMesh.text = "Level: " + currentLevel + "\nDifficulty: " + difficulty.ToString() + "\nAttempts: " + attempts;
        highestLevelMesh.text = "Highest Level: " + highestLevel;

        SaveSystem.SaveData();
    }

    public void flavorText(string txt)
    {
        tempFlavorTextMesh.transform.DOKill();
        tempFlavorTextMesh.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        tempFlavorTextMesh.text = txt;

        if (txt.Equals("Flop!"))
        {
            tempFlavorTextMesh.transform.DOScale(new Vector3(1, 1, 1), 1).OnComplete(shakeText);
        }
        else
        {
            tempFlavorTextMesh.transform.DOScale(new Vector3(1, 1, 1), 1);
        }
        
    }

    public void shakeText()
    {
        tempFlavorTextMesh.transform.DOShakeRotation(1, new Vector3(0, 0, 5f), 10, 30, true);
    }

    public void exitButton()
    {
        gameStart = false;
    }

    public void continueButton()
    {
        gameStart = true;
    }

    public void ResetValues(int diffIndex)
    {
        currentLevel = 1;
        highestLevel = 1;
        attempts = 0;

        gameStart = true;
        difficulty = (Difficulties)diffIndex;

        tempFlavorTextMesh.text = "";
        levelTextMesh.text = "Level: " + currentLevel + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1) + "\nAttempts: " + attempts;
        highestLevelMesh.text = "Highest Level: " + highestLevel;
    }
}
