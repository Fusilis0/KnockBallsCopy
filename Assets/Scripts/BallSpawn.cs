using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallSpawn : MonoBehaviour
{
    public GameObject ballPrefab;
    private GameObject cloneBall;
    public Transform spawnerPos;

    public float ballPower;
    public int numOfBalls;
    public int defaultNumOfBalls;

    public Text uiRemainingBalls;

    private Plane plane;
    public Transform target;
    public float targetDist;
    public Vector3 distFromCam;

    private bool gameStarted = false;

    public GameManager gameManager;

    public AudioSource audioSource;
    public AudioClip spawnClip;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            uiRemainingBalls.text = numOfBalls.ToString();
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            SpawnBall();
            Aim();
        }
    }

    void SetGameStarted() // Sets boolean value to true if the game started
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            gameStarted = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameStarted = false;
        }
    }

    void SpawnBall() // Spawn balls by touch input from prefab
    {
        SetGameStarted();
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began && numOfBalls > 0 && gameStarted)
        {
            cloneBall = Instantiate(ballPrefab, spawnerPos.position, spawnerPos.rotation);
            Destroy(cloneBall, 4.0f);

            UpdateBallCount();
            audioSource.PlayOneShot(spawnClip);
        }
    }

    void Aim() // Sets direction for the spawned balls.
    {
        plane = new Plane(Vector3.forward, distFromCam);

        Touch touch = Input.GetTouch(0);
        Vector3 dir = target.position - spawnerPos.position;

        if (touch.phase == TouchPhase.Ended && gameStarted)
        {
            cloneBall.GetComponent<Rigidbody>().AddForce(dir * ballPower, ForceMode.Impulse);
        }

        float dist = 60.0f;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        if (plane.Raycast(ray, out dist))
        {
            Vector3 point = ray.GetPoint(dist);
            target.position = new Vector3(point.x, point.y, targetDist);
        }
    }

    void UpdateBallCount() // Updates the UI with the current number of balls and handles game over scenario.
    {
        numOfBalls--;

        uiRemainingBalls.text = numOfBalls.ToString();

        if (numOfBalls <= 0)
        {
            Invoke(nameof(CallRestart), 5.0f);
            Invoke(nameof(ResetNumOfBalls), 5.0f);
        }
    }

    public void ResetNumOfBalls() // Sets the number of balls to the default value when starting a new level.
    {
        numOfBalls = defaultNumOfBalls;
    }

    public void CallRestart() // Calls LevelLose (restart game) method from GameManager script.
    {
        gameManager.LevelLose();
    }
}
