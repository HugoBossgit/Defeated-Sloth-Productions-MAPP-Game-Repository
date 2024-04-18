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
        // Uppdatera texten när spelet startar
        UpdateScoreText();
    }

    // Metod för att öka poängen
    public void IncrementScore(int incrementValue)
    {
        score += incrementValue;
        UpdateScoreText();
    }

    // Metod för att uppdatera texten
    void UpdateScoreText()
    {
        // Uppdatera texten med den aktuella poängen
        scoreText.text = "" + score;
    }

    // Metod för att hämta poängen
    public int GetScore()
    {
        return score;
    }
}
