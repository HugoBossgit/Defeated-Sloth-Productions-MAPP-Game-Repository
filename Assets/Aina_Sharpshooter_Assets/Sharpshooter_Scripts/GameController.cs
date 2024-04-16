using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private float multiplier = 1.0f;
    private int comboStarter;
    [SerializeField] private int lives = 3;

    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text livesText;
    public Boolean gameOver;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(lives < 0)
        {
            gameOver = true;
        }
    }

    public void Loss()
    { }

    public void Win()
    { }

    public void Restart()
    { }

}
