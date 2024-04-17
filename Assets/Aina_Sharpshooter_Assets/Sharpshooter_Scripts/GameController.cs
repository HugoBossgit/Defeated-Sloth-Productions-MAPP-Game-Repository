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
    public bool gameOver;

    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameObject tempLossText;
    [SerializeField] private GameObject tempWinText;
    [SerializeField] private GameObject tempInfoText;
    [SerializeField] private TMP_Text livesText;
    void Start()
    {
        tempLossText.SetActive(false);
        tempWinText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoints(int value)
    {
        tempInfoText.SetActive(false);
        points += (int)(value * multiplier);

        if(points > 40)
        {
            gameOver = true;
            tempWinText.SetActive(true);

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

        if(lives < 1)
        {
            gameOver = true;
            tempLossText.SetActive(true);
        }

    }

    public void Loss()
    { }

    public void Win()
    { }

    public void Restart()
    { }

}
