using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Enemy3Behaviour : MonoBehaviour
{
    //Variabler & objekt
    [SerializeField] private Transform target1, target2, target3, currentTarget;
                     private float speed = 0.3f;
                     private int damage = 1;
                     private int lives = 15;
                     private int worth = 100;
                     private bool dying;
    private bool traversing;
    [SerializeField] private float deathTime = 0.3f;
    [SerializeField] private GameController controller;
    private SpriteRenderer renderer;
    private Animator anim;
    void Start()
    {
        traversing = true;
        currentTarget = target1;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }

    /* Spelobjektet r�r sig mot target1:s y-position, sedan mot target2:s, vid 2 kallas decrementLives p�
      (h�lsa s�nks d� fienden �r vid spelaren). Sedan r�r sig objektet mot target3 d�r det sedan f�rst�rs/despawnar */
    void FixedUpdate()
    {
        if (controller.getReady() && transform.position.y < 4)
        {

            if (transform.position.y == target1.position.y)
            {
                currentTarget = target2;
            }

            if (currentTarget == target2 && transform.position.y < target2.position.y + 0.1f)
            {
                traversing = false;
                Invoke("revert", 0.1f);
            }

            if (transform.position.y == target2.position.y)
            {
                currentTarget = target3;
                controller.DecrementLives(damage);
            }

            if (transform.position.y == target3.position.y)
            {
                controller.addDeleted();
                Destroy(gameObject);
            }
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
            anim.SetBool("Traversing", traversing);
        }
    }

    /*N�r muspekaren �r �ver en fiende och man trycker p� "fire1" och gameOver boolen i controller inte �r false:
     S�tts startinformationen inaktiv, combo-metoden och addPoints i controller kallas p� och detta objekt f�rst�rs. */
    void OnMouseOver()
    {
        if (controller.getReady())
        {

            if (Input.GetButtonUp("Fire1") && !controller.gameOver)
            {
                lives--;
                renderer.color = Color.red;
                Invoke("colorBack", 0.5f);
                if(lives <= 0 && !controller.gameOver && !dying)
                {
                    dying = true;
                    currentTarget = target1;
                    controller.addDeleted();
                    controller.makeNoise("Ogre");
                    controller.infoBalloon.SetActive(false);
                    controller.combo(true);
                    controller.addPoints(worth);
                    Destroy(gameObject, 0.1f);
                }
            }
        }

    }
    private void revert()
    {
        traversing = true;
    }
    private void colorBack()
    {
        renderer.color = Color.white;
    }

}