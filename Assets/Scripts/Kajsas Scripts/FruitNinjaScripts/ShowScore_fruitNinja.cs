using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore_fruitNinja : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;

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
    }

    public int GetScore()
    {
        return score;
    }
}
