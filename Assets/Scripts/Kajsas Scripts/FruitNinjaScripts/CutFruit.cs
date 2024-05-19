using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShowScore_fruitNinja;

public class CutFruit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cut")
        {
            Destroy(this.gameObject);
            GameObject scoreText = GameObject.Find("ShowScore");
            scoreText.GetComponent<ShowScore_fruitNinja>().IncrementScore(1);
        }
    }
}
