/* < 8 - 7 - 2022 >
 * Hussien kenaan
 * 
 * This script is for Managing the game loop and UI
 */
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;
    private int level;
    private int layersCleared;

    private bool gameIsOver;

    private float fallSpeed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetScore(score);
    }

    public float ReadFallSpeed()
    {
        return fallSpeed;
    }

    //update local variables from outside
    public void SetScore(int amount)
    {
        score += amount;
        CalculateLevel();
        UIHandler.Instance.UpdateUI(score, level, layersCleared);
    }
    public void SetlayerCleared(int amount)
    {
        if (amount == 1)
        {
            SetScore(400);
        }
        else if (amount == 2)
        {
            SetScore(800);
        }
        else if (amount == 3)
        {
            SetScore(1600);
        }
        if (amount == 4)
        {
            SetScore(3200);
        }
        layersCleared += amount;
        UIHandler.Instance.UpdateUI(score, level, layersCleared);

    }

    //calculate the current speed
    public void CalculateLevel()
    {
        if (score <= 10000)
        {
            level = 1;
            fallSpeed = 3f;
        }
        else if (score > 10000 && score <= 20000)
        {
            level = 2;
            fallSpeed = 2.75f;
        }
        else if (score > 20000 && score <= 30000)
        {
            level = 3;
            fallSpeed = 2.5f;
        }
        else if (score > 30000 && score <= 40000)
        {
            level = 4;
            fallSpeed = 2.25f;
        }
        else if (score > 40000 && score <= 50000)
        {
            level = 5;
            fallSpeed = 2f;
        }
        else if (score > 50000 && score <= 60000)
        {
            level = 6;
            fallSpeed = 1.75f;
        }
        else if (score > 60000 && score <= 70000)
        {
            level = 7;
            fallSpeed = 1.5f;
        }
        else if (score > 70000 && score <= 80000)
        {
            level = 8;
            fallSpeed = 1.25f;
        }
        else if (score > 80000 && score <= 90000)
        {
            level = 9;
            fallSpeed = 1.15f;
        }
        else if (score > 90000)
        {
            level = 2;
            fallSpeed = 1f;
        }
        UIHandler.Instance.UpdateUI(score, level, layersCleared);

    }

    public bool ReadGameOver()
    {
        return gameIsOver;
    }
    public void SetGameOver()
    {
        gameIsOver = true;
        UIHandler.Instance.ActivateGameOverWindow();
    }

}
