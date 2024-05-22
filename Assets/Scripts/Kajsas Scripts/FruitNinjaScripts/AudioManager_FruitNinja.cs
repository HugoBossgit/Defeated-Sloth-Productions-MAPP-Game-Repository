using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerFruitNinja : MonoBehaviour
{
    private AudioSource musicAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeInMusic(musicAudioSource, 3f));
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
