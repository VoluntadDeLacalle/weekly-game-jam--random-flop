using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public TMPro.TextMeshProUGUI levelTextMesh;
    public TMPro.TextMeshProUGUI tempFlavorTextMesh;

    public int maxLevels;
    public int currentLevel;

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
        {   {Difficulties.Easy, 0.5f},
            {Difficulties.Medium, 0.35f},
            {Difficulties.Hard, 0.2f},
            {Difficulties.Insane, 0.1f},
            {Difficulties.Impossible, 0.01f}
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
        hasWon = false;
        gameStart = false;
    }

    void Start()
    {
        levelTextMesh.text = "Level: " + currentLevel;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && gameStart && !hasWon)
        {
            tempFlavorTextMesh.transform.DOScale(new Vector3(0.0001f, 0.0001f, 0.0001f), 0.25f).OnComplete(tryNextLevel);
            //tryNextLevel();
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
            Debug.Log("Next Level!");
            currentLevel++;

            if (currentLevel > maxLevels)
            {
                levelTextMesh.text = "You win!! ...Somehow?";
                hasWon = true;

                flavorText("Winner!");

                return;
            }

            flavorText("Next Level!");
        }
        else
        {
            Debug.Log("Flop!");
            currentLevel = 1;

            flavorText("Flop!");
        }

        levelTextMesh.text = "Level: " + currentLevel;
    }

    public void flavorText(string txt)
    {

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
}
