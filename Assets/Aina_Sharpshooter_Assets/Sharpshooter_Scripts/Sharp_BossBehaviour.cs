using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class BossBehaviour : MonoBehaviour
{
    //Variabler & objekt
    [SerializeField] private Transform target1, target2, target3, target4, target5, target6, currentTarget;
                     private float speed = 1f;
                     private int damage = 1;
                     private int lives = 35;
                     private int worth = 1000;
                     private bool dying;
    private bool traversing;
    [SerializeField] private float deathTime = 0.3f;
    [SerializeField] private GameController controller;
    private SpriteRenderer renderer;
    private Animator anim;
    void Start()
    {
        currentTarget = target1;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        traversing = true;
    }

    /* Spelobjektet rör sig mot target1:s y-position, sedan mot target2:s, vid 2 kallas decrementLives på
      (hälsa sänks då fienden är vid spelaren). Sedan rör sig objektet mot target3 där det sedan förstörs/despawnar */
    void FixedUpdate()
    {
        if (controller.getReady() && transform.position.y < 4)
        {

            if (transform.position.y == target1.position.y)
            {
                currentTarget = target2;
            }

            if (transform.position.y == target2.position.y)
            {
               
                currentTarget = target3;
                
            }

            if (transform.position.y == target3.position.y)
            {
                currentTarget = target4;
            }

            if (transform.position.y == target4.position.y)
            {
                currentTarget = target5;
            }

            if (currentTarget == target5 && transform.position.y < target5.position.y + 0.5f)
            {
                traversing = false;
                Invoke("revert", 0.15f);
            }

            if (transform.position.y == target5.position.y)
            {
                currentTarget = target6;
                controller.DecrementLives(damage);
            }

            if (transform.position.y == target6.position.y)
            {
                currentTarget = target1;
            }

            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
            anim.SetBool("Traversing", traversing);
        }
    }

    /*När muspekaren är över en fiende och man trycker på "fire1" och gameOver boolen i controller inte är false:
     Sätts startinformationen inaktiv, combo-metoden och addPoints i controller kallas på och detta objekt förstörs. */
    void OnMouseOver()
    {
        if (controller.getReady())
        {

            if (Input.GetButtonUp("Fire1") && !controller.gameOver)
            {

                    lives--;
                renderer.color = Color.red;
                Invoke("colorBack", 0.5f);
                if (lives <= 0 && !controller.gameOver && !dying)
                {
                    dying = true;
                    currentTarget = target1;
                    controller.addDeleted();
                    controller.makeNoise("Boss");
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