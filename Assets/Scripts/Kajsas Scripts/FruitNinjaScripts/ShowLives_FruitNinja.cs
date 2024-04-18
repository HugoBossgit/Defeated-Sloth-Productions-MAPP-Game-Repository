using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLives_FruitNinja : MonoBehaviour
{
    [SerializeField] private int numberOfLives;

    [SerializeField] private GameObject lifePrefab;

    private List<GameObject> lives;

    [SerializeField] private GameObject scoreText, gameOverGroup;

    void Start()
    {
        lives = new List<GameObject>();
        for (int lifeIndex = 0; lifeIndex < this.numberOfLives; lifeIndex++)
        {
            GameObject life = Instantiate(lifePrefab, this.gameObject.transform);
            lives.Add(life);
        }
    }

    public void LooseLife()
    {
        this.numberOfLives -= 1;
        GameObject life = this.lives[this.lives.Count - 1];
        this.lives.RemoveAt(this.lives.Count - 1);
        Destroy(life);
        if (this.numberOfLives == 0)
        {
            this.scoreText.SetActive(false);
            this.gameOverGroup.SetActive(true);
        }
    }
}
