using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLives_FruitNinja : MonoBehaviour
{
    public FruitSpawner fruitSpawner;
    public FruitSpawner bombSpawner;

    [SerializeField] private int numberOfLives;

    [SerializeField] private GameObject lifePrefab;

    [SerializeField] private GameObject scoreText, losePanel;

    private List<GameObject> lives;

    private AudioSource musicAudioSource;


    void Start()
    {
        lives = new List<GameObject>();
        for (int lifeIndex = 0; lifeIndex < numberOfLives; lifeIndex++)
        {
            GameObject life = Instantiate(lifePrefab, gameObject.transform);
            lives.Add(life);
        }

        musicAudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();

    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void LooseLife()
    {
        numberOfLives -= 1;
        GameObject life = lives[lives.Count - 1];
        lives.RemoveAt(lives.Count - 1);
        Destroy(life);
        if (numberOfLives == 0)
        {
            StartCoroutine(FadeOutMusic(musicAudioSource, 3f));
            Data.playerLose = true;
            scoreText.SetActive(false);
            losePanel.SetActive(true);
            fruitSpawner.SetGameStatus(false);
            bombSpawner.SetGameStatus(false);
        }
    }
}
