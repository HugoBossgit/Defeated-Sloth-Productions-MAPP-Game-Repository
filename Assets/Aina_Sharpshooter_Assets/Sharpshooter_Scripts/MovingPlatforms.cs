using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private Transform bound1, bound2;
    [SerializeField] private float moveSpeed = 2.0f;

    private Transform currentBound;
    void Start()
    {
        currentBound = bound1;
    }


    void FixedUpdate()
    {
        if(transform.position == bound1.position)
        {
            currentBound = bound2;
        }

        if(transform.position == bound2.position) 
        {
            currentBound = bound1;
        }

        transform.position = Vector2.MoveTowards(transform.position, currentBound.position, moveSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collide)
    {
        if(collide.gameObject.CompareTag("Player") && collide.transform.position.y > transform.position.y)
        {
            collide.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collide)
    {
        if (collide.gameObject.CompareTag("Player"))
        {
            collide.transform.SetParent(null);
        }
    }
}
