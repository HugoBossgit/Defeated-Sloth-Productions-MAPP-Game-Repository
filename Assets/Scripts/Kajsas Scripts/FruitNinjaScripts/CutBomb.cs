using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutBomb : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cut")
        {
            Destroy(gameObject, 0.1f);

            GameObject playerLives = GameObject.Find("PlayerLives");
            playerLives.GetComponent<ShowLives_FruitNinja>().LooseLife();
        }
    }
}

