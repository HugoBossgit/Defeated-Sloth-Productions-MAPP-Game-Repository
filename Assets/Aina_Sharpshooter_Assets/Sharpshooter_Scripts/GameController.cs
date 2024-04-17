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
<<<<<<< Updated upstream
=======
        if(points > 40)
        {
            gameOver = true;
            tempWinText.SetActive(true);

        }
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        if(lives < 1)
        {
            gameOver = true;
            tempLossText.SetActive(true);
        }
>>>>>>> Stashed changes
    }

    public void Loss()
    { }

    public void Win()
    { }

    public void Restart()
    { }

}
