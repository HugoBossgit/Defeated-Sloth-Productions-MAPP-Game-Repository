using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Variabler & objekt
    [SerializeField] private Transform target1, target2, target3, currentTarget;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int lives = 1;
    [SerializeField] private int worth = 10;
    [SerializeField] private float deathTime = 0.3f;
    [SerializeField] private GameController controller;

    void Start()
    {
        currentTarget = target1;
    }

    /* Spelobjektet rör sig mot target1:s y-position, sedan mot target2:s, vid 2 kallas decrementLives på
      (hälsa sänks då fienden är vid spelaren). Sedan rör sig objektet mot target3 där det sedan förstörs/despawnar */
    void FixedUpdate()
    {
        if(transform.position.y == target1.position.y)
        { 
            currentTarget = target2; 
        }

        if (transform.position.y == target2.position.y)
        {
            currentTarget = target3;
            controller.DecrementLives(damage);
        }

        if (transform.position.y == target3.position.y)
        {
            Destroy(gameObject);
        }
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }

    /*När muspekaren är över en fiende och man trycker på "fire1" och gameOver boolen i controller inte är false:
     Sätts startinformationen inaktiv, combo-metoden och addPoints i controller kallas på och detta objekt förstörs. */
    void OnMouseOver()
    {
        if (Input.GetButtonUp("Fire1") && !controller.gameOver)
        {
            controller.infoBalloon.SetActive(false);
            controller.combo(true);
            controller.addPoints(worth);
            Destroy(gameObject);
        }

    }

}