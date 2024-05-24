using System.Collections;
using UnityEngine;
using TMPro;

public class FishingMiniGame : MonoBehaviour
{
    public GameObject GameOverMenu;
    public GameObject WinMenu;
    public TextMeshProUGUI failTimerText;

    [SerializeField] private Transform topPivot;
    [SerializeField] private Transform bottomPivot;
    [SerializeField] private Transform fish;
    [SerializeField] private Transform progressBarContainer;
    [SerializeField] private Transform hook;

    [SerializeField] private float hookSize = 0.1f;
    [SerializeField] private float hookPower = 5f;
    [SerializeField] private float hookPullPower = 0.01f;
    [SerializeField] private float hookGravity = 0.005f;
    [SerializeField] private float hookProgressDegradationPower = 1f;
    [SerializeField] private float timerMultiplicator;
    [SerializeField] private float smoothMotion = 1f;
    [SerializeField] private float failTimer = 10f;

    [SerializeField] private AudioClip fishSplashing;

    private float fishPosition;
    private float fishDestination;
    private float fishTimer;
    private float fishSpeed;
    private float hookPosition;
    private float hookProgress;
    private float hookPullVelocity;

    private bool pause = false;
    private bool isPlayingTimerAnimation = false;

    private AudioSource fishSFXAudioSource;
    private AudioSource musicAudioSource;

    public Animator timerAnim;

    private void Start()
    {
        fishPosition = Random.value;
        UpdateFishPosition();
        fishSFXAudioSource = GetComponent<AudioSource>();
        musicAudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        StartCoroutine(FadeInMusic(musicAudioSource, 2f));
    }

    private void Update()
    {
        if (pause)
            return;

        Fish();
        Hook();
        ProgressCheck();
        UpdateFailTimerUI();
        PlayFishSplashingSound();
    }

    private void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max)
            hookProgress += hookPower * Time.deltaTime;
        else
        {
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;
            failTimer -= Time.deltaTime;

            if (failTimer < 0.4f)
                StartCoroutine(PlayTimerAnim());
        }

        if (hookProgress >= 1f)
            Win();

        hookProgress = Mathf.Clamp01(hookProgress);
    }

    private void Fish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    private void Hook()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
            hookPullVelocity += hookPullPower * Time.deltaTime;

        hookPullVelocity -= hookGravity * Time.deltaTime;
        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f ||
            hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
            hookPullVelocity = 0f;

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }


    private void UpdateFishPosition()
    {
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    private void UpdateFailTimerUI()
    {
        if (isPlayingTimerAnimation == false && failTimer < 0.5f)
            StartCoroutine(PlayTimerAnim());

        failTimerText.text = Mathf.Max(0, Mathf.RoundToInt(failTimer)).ToString();
    }

    private void PlayFishSplashingSound()
    {
        if (!pause && hookProgress > 0 && hookProgress < 1 && !fishSFXAudioSource.isPlaying)
            fishSFXAudioSource.PlayOneShot(fishSplashing);
    }

    private IEnumerator PlayTimerAnim()
    {
        isPlayingTimerAnimation = true;
        StartCoroutine(FadeOutMusic(musicAudioSource, 5f));
        timerAnim.SetTrigger("Shake");
        yield return new WaitForSeconds(0.5f);
        Lose();
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
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

    private void Win()
    {
        Data.playerWin = true;
        pause = true;
        WinMenu.SetActive(true);
        StartCoroutine(FadeOutMusic(musicAudioSource, 3f));
        if (fishSFXAudioSource.isPlaying)
            fishSFXAudioSource.Stop();
    }

    private void Lose()
    {
        StartCoroutine(LoseRoutine());
    }

    private IEnumerator LoseRoutine()
    {
        yield return StartCoroutine(FadeOutMusic(musicAudioSource, 3f));

        Data.playerLose = true;
        pause = true;
        GameOverMenu.SetActive(true);

        if (fishSFXAudioSource.isPlaying)
            fishSFXAudioSource.Stop();
    }
}