using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Enemy2Behaviour : MonoBehaviour
{
    //Variabler & objekt
    [SerializeField] private Transform target1, target2, target3, currentTarget;
                     private float speed = 1.25f;
                     private int damage = 1;
                     private int lives = 2;
                     private int worth = 20;
                     private bool dying;
    [SerializeField] private float deathTime = 0.3f;
    [SerializeField] private GameController controller;
    [SerializeField] private AudioClip[] death, attack;

    private AudioSource audSour;

    void Start()
    {
        currentTarget = target1;
        audSour = GetComponent<AudioSource>();
    }

    /* Spelobjektet r�r sig mot target1:s y-position, sedan mot target2:s, vid 2 kallas decrementLives p�
      (h�lsa s�nks d� fienden �r vid spelaren). Sedan r�r sig objektet mot target3 d�r det sedan f�rst�rs/despawnar */
    void FixedUpdate()
    {
        if (controller.getReady() && transform.position.y < 15)
        {
            if (transform.position.y == target1.position.y)
            {
                currentTarget = target2;
            }

            if (transform.position.y == target2.position.y)
            {
                makeNoise("Attacking");
                currentTarget = target3;
                controller.DecrementLives(damage);
            }

            if (transform.position.y == target3.position.y)
            {
                controller.addDeleted();
                Destroy(gameObject);
            }
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
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
                if(lives <= 0 && !controller.gameOver && !dying)
                {
                    dying = true;
                    currentTarget = target1;
                    controller.addDeleted();
                    makeNoise("Dead");
                    controller.infoBalloon.SetActive(false);
                    controller.combo(true);
                    controller.addPoints(worth);
                    Destroy(gameObject, 0.1f);
                }
            }
        }

    }

    private void makeNoise(string s)
    {
        int deathIndex = Random.Range(1, death.Length);
        int attackIndex = Random.Range(1, attack.Length);
        if (s.Equals("Dead"))
        {
            AudioClip deathClip = death[deathIndex];
            audSour.PlayOneShot(deathClip);
            death[deathIndex] = death[0];
            death[0] = deathClip;
        }

        if (s.Equals("Attacking"))
        {
            AudioClip attackClip = attack[attackIndex];
            audSour.PlayOneShot(attackClip);
            attack[deathIndex] = attack[0];
            attack[0] = attackClip;
        }
    }

}