using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private float multiplier = 1.0f;
    [SerializeField] private int lives = 3;

    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject lossBalloon;
    [SerializeField] private GameObject winBalloon;
    [SerializeField] private GameObject readyUpUI;
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    public GameObject infoBalloon;
    
    public bool gameOver, gameWon;
    private bool ready;
    private int comboStarter;
    private int deleted;
    void Start()
    {
        lossBalloon.SetActive(false);
        winBalloon.SetActive(false);
        infoBalloon.SetActive(true);
        readyUpUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemies.Count == deleted && !gameOver)
        {
            Win();
        }
        
    }

    public void addPoints(int value)
    {
        points += (int)(value * multiplier);
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
        gameWon = true;
        winBalloon.SetActive(true);
    }

    public void Restart()
    { }

    public void startGame()
    {
        readyUpUI.SetActive(false);
        ready = true;
    }

    public bool getReady()
    { 
        return ready; 
    }

    public void addDeleted()
    {
        deleted++;
    }

}
