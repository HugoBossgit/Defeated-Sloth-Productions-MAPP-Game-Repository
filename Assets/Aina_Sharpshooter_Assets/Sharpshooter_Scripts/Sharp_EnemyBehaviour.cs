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

    /* Spelobjektet r�r sig mot target1:s y-position, sedan mot target2:s, vid 2 kallas decrementLives p�
      (h�lsa s�nks d� fienden �r vid spelaren). Sedan r�r sig objektet mot target3 d�r det sedan f�rst�rs/despawnar */
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

    /*N�r muspekaren �r �ver en fiende och man trycker p� "fire1" och gameOver boolen i controller inte �r false:
     S�tts startinformationen inaktiv, combo-metoden och addPoints i controller kallas p� och detta objekt f�rst�rs. */
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