using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWizard : MonoBehaviour
{

    public float moveSpeed;

    Rigidbody2D rb;

    [SerializeField] private GameObject winInfo;
    [SerializeField] private GameObject loseInfo;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        loseInfo.SetActive(false);
        winInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (touchPos.x < 0)
            {
                rb.AddForce(Vector2.left * moveSpeed);
            }
            else
            {
                rb.AddForce(Vector2.right * moveSpeed);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            Data.playerLose = true;
            loseInfo.SetActive(true);
            StartCoroutine(Delay(0.1f));

            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator Delay(float duration)
    {
        yield return new WaitForSeconds(duration);

    }



}
