using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HunterBehavior : MonoBehaviour
{

    [SerializeField] GameController controller;
    private SpriteRenderer renderer;
    private Animator anim;
    private int checker, counter;
    private bool toCheck;
    public bool attacking;

    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Fire1"))
        {
            attacking = true;
            counter++;
            Invoke("revert", 0.15f);
        }
        anim.SetBool("Attacking", attacking);
        
    }

    private void revert()
    {
        attacking = false;

    }

    public void hurt()
    {
        renderer.color = Color.red;
        Invoke("colorBack", 0.5f);
    }
    private void colorBack()
    {
        renderer.color = Color.white;
    }
}
