using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShowLives_FruitNinja;

public class CutBomb : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cut")
        {
            Destroy(this.gameObject);
            GameObject playerLives = GameObject.Find("PlayerLives");
            playerLives.GetComponent<ShowLives_FruitNinja>().LooseLife();
        }
    }
}
