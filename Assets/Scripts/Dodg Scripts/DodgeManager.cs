using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeManager : MonoBehaviour
{

    public GameObject fireBall;
    public float maxX;
    public Transform spawnPoint;
    public float spawnRate;

    [SerializeField] private GameObject startInfo;


    bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        startInfo.SetActive(true);

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
    }



}
