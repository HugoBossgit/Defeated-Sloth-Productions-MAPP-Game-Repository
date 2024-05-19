using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CreateCuts : MonoBehaviour
{
    [SerializeField] private GameObject cut;
    [SerializeField] private float cutDestroyTime;
    [SerializeField] private AudioClip[] cutSounds;
    [SerializeField] private AudioClip explosionSound;

    private AudioSource audioSource;

    private bool dragging = false;

    private Vector2 swipeStart;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            swipeStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            createCut();
        }
    }
    private void createCut()
    {
        dragging = false;

        //beräknar slutpositionen för svajpet
        Vector2 swipeEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //skapar skurnignen och sätter start/slutposition med linerendeerer
        GameObject cut = Instantiate(this.cut, swipeStart, Quaternion.identity);
        cut.GetComponent<LineRenderer>().SetPosition(0, swipeStart);
        cut.GetComponent<LineRenderer>().SetPosition(1, swipeEnd);

        //skapar kollisionspunkter för skärningens collider
        Vector2[] colliderPoints = new Vector2[2];
        colliderPoints[0] = new Vector2(0.0f, 0.0f);
        colliderPoints[1] = swipeEnd - swipeStart;
        cut.GetComponent<EdgeCollider2D>().points = colliderPoints;

        //förstör cutten efter en viss tid så att de inte ligger kvar i scen
        Destroy(cut.gameObject, cutDestroyTime);

        // Kolla kollision med frukt och spela ljud
        RaycastHit2D[] hits = Physics2D.RaycastAll(swipeStart, swipeEnd - swipeStart);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Fruit"))
            {
                int randomValue = Random.Range(0, cutSounds.Length);
                audioSource.volume = 0.5f;
                audioSource.PlayOneShot(cutSounds[randomValue]);
                break;
            }else if (hit.collider != null && hit.collider.CompareTag("Bomb"))
            {
                audioSource.pitch = Random.Range(0.8f, 1.5f);
                audioSource.volume = 0.5f;
                audioSource.PlayOneShot(explosionSound);
                break;
            }
        }
    }
}