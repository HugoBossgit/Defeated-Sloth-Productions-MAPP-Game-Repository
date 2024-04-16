using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
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

    void OnMouseOver()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            controller.combo(true);
            controller.addPoints(worth);
            Destroy(gameObject);
        }

    }

}