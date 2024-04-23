using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private float multiplier = 1.0f;
    private int comboStarter;
    [SerializeField] private int lives = 3;

    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject lossBalloon;
    [SerializeField] private GameObject winBalloon;
    public GameObject infoBalloon;
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    public Boolean gameOver;
    void Start()
    {
        lossBalloon.SetActive(false);
        winBalloon.SetActive(false);
        infoBalloon.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0)
        {
            Win();
        }

        for(int i = enemies.Count - 1; i >= 0; i--) 
        {
            if (!enemies[i].CompareTag("Enemy"))
            {
                enemies.RemoveAt(i);
            }
        }
    }

    public void addPoints(int value)
    {
        points += (int)(value * multiplier);
        if(points > 40)
        {
            gameOver = true;
        }
    }

    public void combo(Boolean increase)
    {
        comboStarter++;
        if (increase && comboStarter > 2) 
        {
            multiplier += 0.1f;
        }
        
        if(!increase)
        {
            multiplier = 1.0f;
        }
    }

    public void DecrementLives(int decrease)
    {
        lives -= decrease;
        if (lives >= 0)
        {
            Destroy(hearts.ElementAt(lives));
            //hearts.RemoveAt(lives);
        }
        
        if(lives <= 0)
        {
            Loss();
        }
    }

    public void Loss()
    {
        gameOver = true;
        lossBalloon.SetActive(true);
    }

    public void Win()
    {
        winBalloon.SetActive(true);
    }

    public void Restart()
    { }

}
