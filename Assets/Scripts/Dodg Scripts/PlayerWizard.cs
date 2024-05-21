using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerWizard : MonoBehaviour
{
    public int score = 0;

    public float moveSpeed;

    Rigidbody2D rb;

    [SerializeField] private GameObject loseInfo;

    private AudioSource backgroundMusic;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        backgroundMusic = GetComponent<AudioSource>();
        loseInfo.SetActive(false);
        StartCoroutine(FadeInMusic(backgroundMusic, 3f));

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
            CancelInvoke("SpawnFireBall");

        }
    }



    private IEnumerator Delay(float duration)
    {
        yield return new WaitForSeconds(duration);

    }

    private IEnumerator FadeInMusic(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0f;
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.volume = startVolume;
    }





}
