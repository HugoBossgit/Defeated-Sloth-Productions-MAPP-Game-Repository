using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;

    [SerializeField] float spawnInterval, objectMinX, objectMaxX, objectY;

    [SerializeField] Sprite[] objectSprites;

    void Start()
    {
        InvokeRepeating("spawnObject", this.spawnInterval, this.spawnInterval);
    }

    private void spawnObject()
    {
        GameObject newObject = Instantiate(this.prefabToSpawn);
        newObject.transform.position = new Vector2(Random.Range(this.objectMinX, this.objectMaxX), this.objectY);

        Sprite objectSprite = objectSprites[Random.Range(0, this.objectSprites.Length)];
        newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
    }

   
}
