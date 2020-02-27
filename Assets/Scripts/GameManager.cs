using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Camera mainCam;

    public Canvas canvas;
    public TMPro.TextMeshProUGUI levelTextMesh;
    public TMPro.TextMeshProUGUI tempFlavorTextMesh;
    public TMPro.TextMeshProUGUI highestLevelMesh;
    public TMPro.TextMeshProUGUI level1Text;
    public TMPro.TextMeshProUGUI level10Text;

    public GameObject levelSliderPanel;
    public Slider levelSlider;
    public Button backButton;

    private Transform[] shakeables;
    private Transform[] originalShakeableTransforms;

    public AudioSource audSource;
    public AudioClip nextClip;
    public AudioClip bruhClip;
    public AudioClip winClip;

    public int maxLevels;
    public int currentLevel;
    public int highestLevel;
    private float levelOffset;
    public int attempts;

    public bool gameStart;
    public bool hasWon;
    public bool canTry;
    public bool shouldColor;

    Sequence colorSequence;
    public float ColorChangeTime;
    public Color[] colorList;
    private Color defaultColor;

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
        levelOffset = (1.0f / (float)maxLevels);
        attempts = 0;

        hasWon = false;
        gameStart = false;
        canTry = true;

        shouldColor = false;
        defaultColor = new Color(159f / 255f, 159f / 255f, 159f / 255f, 0f);

        shakeables = new Transform[] { levelTextMesh.transform, tempFlavorTextMesh.transform, highestLevelMesh.transform, level1Text.transform, level10Text.transform, levelSliderPanel.transform, backButton.transform };
        originalShakeableTransforms = shakeables;
    }

    void Start()
    {
        if (difficulty != Difficulties.Impossible)
        {
            levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1);
        }
        else
        {
            levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1) + "?";
        }

        levelSlider.DOValue(((float)currentLevel / (float)maxLevels) - levelOffset, .75f).Play();
        highestLevelMesh.text = "Highest Level: " + highestLevel;

        mainCam.DOColor(defaultColor, .5f);
    }

    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0)) && gameStart && !hasWon && canTry)
        {
            tempFlavorTextMesh.transform.DOKill();
            doTheKills();
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

    void changeBackgroundColor()
    {
        colorSequence = DOTween.Sequence();
        colorSequence.Append(mainCam.DOColor(colorList[0], ColorChangeTime))
            .AppendCallback(() => { mainCam.backgroundColor = colorList[0]; })
                    .Append(mainCam.DOColor(colorList[1], ColorChangeTime))
            .AppendCallback(() => { mainCam.backgroundColor = colorList[1]; })
                    .Append(mainCam.DOColor(colorList[2], ColorChangeTime))
            .AppendCallback(() => { mainCam.backgroundColor = colorList[2]; })
                    .Append(mainCam.DOColor(colorList[3], ColorChangeTime))
            .AppendCallback(() => { mainCam.backgroundColor = colorList[3]; })
                    .Append(mainCam.DOColor(colorList[4], ColorChangeTime))
            .AppendCallback(() => { mainCam.backgroundColor = colorList[4]; })
                    .Append(mainCam.DOColor(colorList[5], ColorChangeTime))
            .AppendCallback(() => { mainCam.backgroundColor = colorList[5]; }).SetLoops(-1);
    }

    void tryNextLevel()
    {
        float rand = Random.Range(0.0f, 1.0f);
        canTry = false;

        if (rand <= difficultyValues[difficulty])
        {
            currentLevel++;

            if (!shouldColor)
            {
                mainCam.DOColor(colorList[5], ColorChangeTime).OnComplete(changeBackgroundColor);
                shouldColor = true;
            }

            if (currentLevel > maxLevels)
            {
                levelSlider.DOValue(((float)currentLevel / (float)maxLevels) - levelOffset, .75f).Play();
                hasWon = true;

                ColorChangeTime = 1f;
                shouldColor = false;
                colorSequence.Kill();
                mainCam.DOKill();

                if (!shouldColor)
                {
                    mainCam.DOColor(colorList[5], ColorChangeTime).OnComplete(changeBackgroundColor);
                    shouldColor = true;
                }

                if (audSource.isPlaying)
                {
                    audSource.Stop();
                }
                audSource.clip = winClip;
                audSource.Play();

                flavorText("Winner!");

                attempts++;
                if (difficulty != Difficulties.Impossible)
                {
                    levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1);
                }
                else
                {
                    levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1) + "?";
                }

                return;
            }

            if (audSource.isPlaying)
            {
                audSource.Stop();
            }
            audSource.clip = nextClip;
            audSource.Play();

            if (currentLevel > highestLevel)
            {
                highestLevel = currentLevel;
            }

            flavorText("Next Level!");
        }
        else
        {
            if (currentLevel > 1)
            {
                currentLevel--;
            }

            colorSequence.Kill();
            mainCam.DOKill();
            shouldColor = false;
            mainCam.DOColor(defaultColor, .5f);

            doTheShakes();

            if (audSource.isPlaying)
            {
                audSource.Stop();
            }
            audSource.clip = bruhClip;
            audSource.Play();

            flavorText("Flop!");
        }

        attempts++;


        if (difficulty != Difficulties.Impossible)
        {
            levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1);
        }
        else
        {
            levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1) + "?";
        }

        levelSlider.DOValue(((float)currentLevel / (float)maxLevels) - levelOffset, .75f).Play();
        highestLevelMesh.text = "Highest Level: " + highestLevel;

        SaveSystem.SaveData();
    }

    void doTheShakes()
    {
        for (int i = 0; i < shakeables.Length; i++)
        {
            shakeables[i].DOShakePosition(1.2f, 20);
        }
    }

    void doTheKills()
    {
        for (int i = 0; i < shakeables.Length; i++)
        {
            shakeables[i].DOKill(true);
            shakeables[i] = originalShakeableTransforms[i];
        }
    }

    public void flavorText(string txt)
    {
        tempFlavorTextMesh.transform.DOKill();
        tempFlavorTextMesh.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        tempFlavorTextMesh.text = txt;

        if (txt.Equals("Flop!"))
        {
            tempFlavorTextMesh.transform.DOScale(new Vector3(1, 1, 1), .2f).OnComplete(shakeText);
        }
        else
        {
            tempFlavorTextMesh.transform.DOScale(new Vector3(1, 1, 1), .2f).OnComplete(canTryTrue);
        }
        
    }

    public void shakeText()
    {
        tempFlavorTextMesh.transform.DOShakeRotation(.2f, new Vector3(0, 0, 30f), 10, 30, true).OnComplete(canTryTrue);
    }

    public void canTryTrue()
    {
        canTry = true;
    }

    public void exitButton()
    {
        gameStart = false;
        canTry = false;
        shouldColor = false;

        colorSequence.Kill();
        mainCam.DOKill();
        mainCam.DOColor(defaultColor, .5f);
    }

    public void continueButton()
    {
        gameStart = true;
        canTry = true;

        if (currentLevel <= maxLevels)
        {
            tempFlavorTextMesh.transform.DOKill();
            tempFlavorTextMesh.text = "";

            shouldColor = false;
            colorSequence.Kill();
            mainCam.DOKill();
            mainCam.DOColor(defaultColor, .5f);
        }
    }

    public void ResetValues(int diffIndex)
    {
        currentLevel = 1;
        highestLevel = 1;
        attempts = 0;

        gameStart = true;
        canTry = true;
        hasWon = false;
        difficulty = (Difficulties)diffIndex;

        tempFlavorTextMesh.transform.DOKill();
        tempFlavorTextMesh.text = "";

        if (difficulty != Difficulties.Impossible)
        {
            levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1);
        }
        else
        {
            levelTextMesh.text = "Attempts: " + attempts + "\nDifficulty: " + char.ToUpper(difficulty.ToString()[0]) + difficulty.ToString().Substring(1) + "?";
        }

        levelSlider.value = 0;
        highestLevelMesh.text = "Highest Level: " + highestLevel;

        shouldColor = false;
        colorSequence.Kill();
        mainCam.DOKill();
        mainCam.DOColor(defaultColor, .5f);
    }
}
