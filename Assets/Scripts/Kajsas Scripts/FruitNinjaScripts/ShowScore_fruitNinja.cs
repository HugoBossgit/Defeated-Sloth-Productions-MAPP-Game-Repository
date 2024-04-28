using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore_fruitNinja : MonoBehaviour
{
    private int score = 0;

    public TextMeshProUGUI scoreText;
    public GameObject winPanel;

    public FruitSpawner fruitSpawner;
    public FruitSpawner bombSpawner;

    void Start()
    {
        UpdateScoreText();
    }

    public void IncrementScore(int incrementValue)
    {
        score += incrementValue;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "" + score;

        if(score == 30)
        {
            Data.playerWin = true;
            winPanel.SetActive(true);
            fruitSpawner.SetGameStatus(false);
        }
    }

    public int GetScore()
    {
        return score;
    }
}
