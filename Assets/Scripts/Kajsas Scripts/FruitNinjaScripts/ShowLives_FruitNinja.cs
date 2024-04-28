using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLives_FruitNinja : MonoBehaviour
{
    public FruitSpawner fruitSpawner;
    public FruitSpawner bombSpawner;

    [SerializeField] private int numberOfLives;

    [SerializeField] private GameObject lifePrefab;

    private List<GameObject> lives;

    [SerializeField] private GameObject scoreText, losePanel;

    void Start()
    {
        lives = new List<GameObject>();
        for (int lifeIndex = 0; lifeIndex < numberOfLives; lifeIndex++)
        {
            GameObject life = Instantiate(lifePrefab, gameObject.transform);
            lives.Add(life);
        }
    }

    public void LooseLife()
    {
        numberOfLives -= 1;
        GameObject life = lives[lives.Count - 1];
        lives.RemoveAt(lives.Count - 1);
        Destroy(life);
        if (numberOfLives == 0)
        {
            Data.playerLose = true;
            scoreText.SetActive(false);
            losePanel.SetActive(true);
            fruitSpawner.SetGameStatus(false);
        }
    }
}
