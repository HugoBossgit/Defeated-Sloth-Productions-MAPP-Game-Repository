using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShowScore_fruitNinja;

public class CutFruit : MonoBehaviour
{

    [SerializeField] private AudioClip[] glassSounds;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cut")
        {
            int randomValue = Random.Range(0, glassSounds.Length);
            audioSource.PlayOneShot(glassSounds[randomValue]);
            Destroy(this.gameObject);
            GameObject scoreText = GameObject.Find("ShowScore");
            scoreText.GetComponent<ShowScore_fruitNinja>().IncrementScore(1);
        }
    }
}
