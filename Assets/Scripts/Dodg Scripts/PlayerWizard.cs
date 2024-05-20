using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWizard : MonoBehaviour
{

    public float moveSpeed;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
