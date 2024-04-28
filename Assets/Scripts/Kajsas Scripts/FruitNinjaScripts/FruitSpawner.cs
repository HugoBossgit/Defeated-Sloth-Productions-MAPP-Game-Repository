using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;

    [SerializeField] float spawnInterval, objectMinX, objectMaxX, objectY;

    [SerializeField] Sprite[] objectSprites;

    private bool gameIsRunning = true;

    void Start()
    {
        InvokeRepeating("spawnObject", spawnInterval, spawnInterval);
    }

    private void spawnObject()
    {
        if (!gameIsRunning) return;

        //skapr instans av fruktprefab och väljer sedan X-position för given intervall
        GameObject newObject = Instantiate(prefabToSpawn);
        newObject.transform.position = new Vector2(Random.Range(objectMinX, objectMaxX), objectY);

        Sprite objectSprite = objectSprites[Random.Range(0, objectSprites.Length)];
        newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
    }

    public void SetGameStatus(bool status)
    {
        gameIsRunning = status;
    }
}
