using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool cubeExist;
    public LayerMask cubeLayer;
    public float sphereRadius;

    public Score score;

    private bool levelWonCalled = false;

    public GameObject uiLevelWon;
    public GameObject uiLevelLose;
    public GameObject uiFinishGame;
    public GameObject uiTapToPlay;
    public Text uiHighScoreValue;
    public GameObject uiBallNumber;

    void Update()
    {
        CheckCubes();
        TapToPlay();
    }

    void CheckCubes() // Checks around the platform to see if a cube exists; if not, calls the LevelWon method.
    {
        if (Physics.CheckSphere(transform.position, sphereRadius, cubeLayer))
        {
            cubeExist = true;
            levelWonCalled = false;
        }
        else
        {
            cubeExist = false;
            if (!levelWonCalled)
            {
                Invoke("LevelWon", 0.5f);
                levelWonCalled = true;
            }
        }
    }

    void OnDrawGizmosSelected() // For Debugging.
    {
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }

    void TapToPlay() // Main menu implementation.
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            uiHighScoreValue.text = PlayerPrefs.GetInt("HighScore").ToString();

            if (Input.touchCount > 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                uiTapToPlay.SetActive(false);
            }
        }
    }

    public void LevelWon() // If player wins a level, shows UI and then calls for the next level.
    {
        Debug.Log("Level Won!");

        score.SetScore();
        score.SetHighScore();
        score.WriteScoresToUI();

        Invoke("LoadNextLevel", 2.0f);

        if (SceneManager.GetActiveScene().buildIndex < 5)
        {
            uiLevelWon.SetActive(true);
        }

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            uiFinishGame.SetActive(true);
        }

        uiBallNumber.SetActive(false);
    }

    public void LevelLose() // If player loses a level, shows UI and then calls for the main menu.
    {
        Debug.Log("You lose, restart :{");

        score.SetScore();
        score.SetHighScore();
        score.WriteScoresToUI();

        uiLevelLose.SetActive(true);

        Invoke("LoadMainScene", 5.0f);
    }

    public void LoadMainScene() // Loads the main menu.
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel() // Loads the next level.
    {
        if (SceneManager.GetActiveScene().buildIndex < 5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Invoke("LoadMainScene", 5.0f);
        }
    }
}
