using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutBomb : MonoBehaviour
{
    public Animator bombAnim;
    public FruitSpawner fruitSpawner;
    public FruitSpawner bombSpawner;

    private Rigidbody2D rgbd;

    private void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cut")
        {

            rgbd.gravityScale = 0f;

            bombAnim.SetTrigger("Explode");

            fruitSpawner.SetGameStatus(false);
            bombSpawner.SetGameStatus(false);

            Destroy(gameObject, 0.6f);

            GameObject playerLives = GameObject.Find("PlayerLives");
            playerLives.GetComponent<ShowLives_FruitNinja>().LooseLife();
        }
    }

    public void OnExplosionFinished()
    {
        rgbd.gravityScale = 1f;

        fruitSpawner.SetGameStatus(true);
        bombSpawner.SetGameStatus(true);
    }
}

