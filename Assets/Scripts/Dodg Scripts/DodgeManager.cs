using TMPro;
using UnityEngine;

public class DodgeManager : MonoBehaviour
{

    public GameObject fireBall;
    public float maxX;
    public Transform spawnPoint;
    public float spawnRate;
    public TextMeshProUGUI scoreText;

    [SerializeField] private GameObject startInfo;
    [SerializeField] private GameObject winInfo;



    bool gameStarted = false;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        startInfo.SetActive(true);
        winInfo.SetActive(false);

        UpdateScoreText();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            startInfo.SetActive(false);

            StartSpawning();

            gameStarted = true;

        }
    }


    private void StartSpawning()
    {
        InvokeRepeating("SpawnFireBall", 0.5f, spawnRate);
    }


    private void SpawnFireBall()
    {
        Vector3 spawnPos = spawnPoint.position;

        spawnPos.x = Random.Range(-maxX, maxX);

        Instantiate(fireBall, spawnPos, Quaternion.identity);



        score++;
        UpdateScoreText();

        if ( score == 15)
        {
            WinGame();
        }

    }



    void UpdateScoreText()
    {
        scoreText.text = score + "/15";

    }

    private void WinGame()
    {
        Data.playerWin = true;
        winInfo.SetActive(true);
        CancelInvoke("SpawnFireBall");

    }

    public void PlayerLose()
    {
        CancelInvoke("SpawnFireBall");
    }

}
