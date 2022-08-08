/* < 8 - 8 - 2022 >
 * Hussien kenaan
 * 
 * This script is for updating the UI
 */
using UnityEngine;
using TMPro;
public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI layersText;

    [SerializeField] GameObject gameOverWindow;

    private void Start()
    {
        gameOverWindow.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    //update the values
    public void UpdateUI(int score, int level, int layers)
    {
        scoreText.text = "Score: " + score.ToString("");
        levelText.text = "Level: " + level.ToString("D2");
        layersText.text = "Layers: " + layers.ToString("");
    }

    //activate game over window
    public void ActivateGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }
}
