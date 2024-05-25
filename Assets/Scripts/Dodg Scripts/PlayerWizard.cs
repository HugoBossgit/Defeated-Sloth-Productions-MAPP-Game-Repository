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
    [SerializeField] private AudioClip hitSound;

    private AudioSource backgroundMusic;
    private AudioSource loseSource;
    private DodgeManager dodgeManager;
    private bool gameActive = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        backgroundMusic = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        loseSource = GetComponent<AudioSource>();
        loseInfo.SetActive(false);
        StartCoroutine(FadeInMusic(backgroundMusic, 3f));

        dodgeManager = FindObjectOfType<DodgeManager>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!gameActive) return;

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
            
            if (dodgeManager != null)
            {
                dodgeManager.PlayerLose();
            }
            gameActive = false;

            loseSource.PlayOneShot(hitSound);

            FreezeObject(gameObject);
            FreezeAllFireBalls();
        }

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

    public void FreezeObject(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // stops movment
            rb.isKinematic = true; //makes so that its not affected by physics
        }
    }

    public void FreezeAllFireBalls()
    {
        GameObject[] fireballs = GameObject.FindGameObjectsWithTag("FireBall");
        foreach (GameObject fireball in fireballs)
        {
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
        }
    }



}
