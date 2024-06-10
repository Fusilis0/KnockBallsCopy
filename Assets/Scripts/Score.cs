using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public int score;
    public int highScore = 0;

    public BallSpawn ballSpawn;

    public Text uiFgHighScore;
    public Text uiFgScore;

    public Text uiLwScore;

    public Text uiLlHighScore;
    public Text uiLlScore;

    public void SetHighScore() // Sets high scores and stores it locally.
    {
        int savedHighScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }
    }

    public void CalculateScore() // Calculation for score.
    {
        score = ballSpawn.numOfBalls * 15;

        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            score += PlayerPrefs.GetInt("Score");
        }
    }

    public void SetScore() // Set score on the local device.
    {
        CalculateScore();
        PlayerPrefs.SetInt("Score", score);
    }

    public void WriteScoresToUI() // Writes score and high score values to UI elements.
    {
        int currentScore = PlayerPrefs.GetInt("Score");
        int currentHighScore = PlayerPrefs.GetInt("HighScore");

        uiLwScore.text = currentScore.ToString();
        uiLlScore.text = currentScore.ToString();
        uiLlHighScore.text = currentHighScore.ToString();

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            uiFgScore.text = currentScore.ToString();
            uiFgHighScore.text = currentHighScore.ToString();
        }
    }
}
