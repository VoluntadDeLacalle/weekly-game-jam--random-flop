using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public TMPro.TextMeshProUGUI levelTextMesh;

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
            Debug.Log("Next Level!");
            currentLevel++;

            if (currentLevel > maxLevels)
            {
                levelTextMesh.text = "You win!! ...Somehow?";
                hasWon = true;
                return;
            }
        }
        else
        {
            Debug.Log("Flop!");
            currentLevel = 1;
        }

        levelTextMesh.text = "Level: " + currentLevel;
    }
}
