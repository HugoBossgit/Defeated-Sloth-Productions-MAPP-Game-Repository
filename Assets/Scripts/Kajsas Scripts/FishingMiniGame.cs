using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishingMiniGame : MonoBehaviour
{

    public GameObject GameOverMenu;
    public GameObject WinMenu;
    public TextMeshProUGUI failTimerText;

    //Referens till punkterna som fisken kommer röra sig mellan
    [SerializeField] private Transform topPivot;
    [SerializeField] private Transform bottomPivot;

    [SerializeField] private Transform fish;

    private float fishPosition;
    private float fishDestination;

    //bestämmer hur lång tid den är på en viss plats;
    private float fishTimer;
    [SerializeField] private float timerMultiplicator;

    [SerializeField] private float failTimer = 10f;

    //hur snabbt den rör sig mellan punkter
    private float fishSpeed;
    [SerializeField] private float smoothMotion = 1f;

    [SerializeField] Transform progressBarContainer;

    [SerializeField] Transform hook;

    [SerializeField] private float hookSize = 0.1f;
    [SerializeField] private float hookPower = 5f;
    [SerializeField] private float hookPullPower = 0.01f;
    [SerializeField] private float hookGravity = 0.005f;
    [SerializeField] private float hookProgressDegradationPower = 1f;

    [SerializeField] private AudioClip fishSplashing;

    private float hookPosition;
    private float hookProgress;
    private float hookPullVelocity;

    private bool pause = false;

    private AudioSource audioSource;

    private void Start()
    {
        fishPosition = Random.value;

        UpdateFishPosition();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (pause)
        {
            return;
        }

        Fish();
        Hook();
        ProgressCheck();
        UpdateFailTimerUI();

        if (!pause)
        {
            if (hookProgress > 0 && hookProgress < 1 && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fishSplashing);
            }
        }
    }

    private void ProgressCheck()
    {
        //uppdaterar progressBar baserat på fiskens och krokens position
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            //om fisken inte är i hook så kommer hookpower långsamt gå ner
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;

            failTimer -= Time.deltaTime;

            if (failTimer < 0.5f)
            {
                Lose();
            }
        }

        if (hookProgress >= 1f)
        {
            Win();
        }

        //säkerhetsställer att progressen är inom giltiga ramar
        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
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
        if (Input.GetMouseButton(0))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        hookPullVelocity -= hookGravity * Time.deltaTime;

        hookPosition += hookPullVelocity;

        if(hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }

        if(hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    private void UpdateFishPosition()
    {
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    private void UpdateFailTimerUI()
    {
        // Uppdatera UI-textkomponenten för failtimern med den aktuella failtimern
        failTimerText.text = "Fail Timer: " + Mathf.RoundToInt(failTimer).ToString();
    }

    private void Win()
    {
        Data.playerWin = true;
        pause = true;
        WinMenu.SetActive(true);
    }

    private void Lose()
    {
        Data.playerLose = true;
        pause = true;
        GameOverMenu.SetActive(true);
    }
}
