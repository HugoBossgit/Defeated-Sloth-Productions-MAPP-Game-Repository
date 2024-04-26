using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMiniGame : MonoBehaviour
{

    public GameObject GameOverMenu;
    public GameObject WinMenu;

    //Referens till punkterna som fisken kommer röra sig mellan
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    float fishPosition;
    float fishDestination;

    //bestämmer hur lång tid den är på en viss plats;
    float fishTimer;
    [SerializeField] float timerMultiplicator;

    [SerializeField] float failTimer = 10f;

    //hur snabbt den rör sig mellan punkter
    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform progressBarContainer;

    [SerializeField] Transform hook;

    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower = 5f;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravity = 0.005f;
    [SerializeField] float hookProgressDegradationPower = 1f;

    float hookPosition;
    float hookProgress;
    float hookPullVelocity;

    bool pause = false;

    private void Update()
    {
        if (pause)
        {
            return;
        }
        Fish();
        Hook();
        ProgressCheck();
    }

    private void ProgressCheck()
    {
        //uppdaterar progressBar baserat på fiskens och krokens position
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if(min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            //om fisken inte är i hook så kommer hookpower långsamt gå ner
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;

            failTimer -= Time.deltaTime;
            if(failTimer < 0.5f)
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
