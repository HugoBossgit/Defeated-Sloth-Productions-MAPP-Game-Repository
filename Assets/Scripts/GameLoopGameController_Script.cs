using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopGameController_Script : MonoBehaviour
{
    [SerializeField] private GameObject gameProgressSliderGameObject;
    [SerializeField] private GameObject pausedGamePanel;
    [SerializeField] private GameObject playerHealthSliderGameObject;
    [SerializeField] private GameObject walkingButtonGameObject;
    [SerializeField] private GameObject runningButtonGameObject;
    [SerializeField] private GameObject gameWinPanelGameObject;
    [SerializeField] private GameObject gameLosePanelGameObject;
    [SerializeField] private List<GameObject> enemies;

    private Slider playerProgressSlider, playerHealthSlider;

    private Button walkingButton, runningButton;

    private bool gameIsPaused;

    //Antal steg till spel loopens slut i walking pace (1/1) vilket blir 60 sekunder
    private int maxGameProgress = 60;
    private int maxPlayerHealth = 10;

    private void Start()
    {
        pausedGamePanel.SetActive(false);
        gameWinPanelGameObject.SetActive(false);
        gameLosePanelGameObject.SetActive(false);

        gameIsPaused = false;


        if (gameProgressSliderGameObject != null)
        {
            playerProgressSlider = gameProgressSliderGameObject.GetComponent<Slider>();
            playerProgressSlider.maxValue = maxGameProgress;
        }

        if (playerHealthSliderGameObject != null)
        {
            playerHealthSlider = playerHealthSliderGameObject.GetComponent<Slider>();
            playerHealthSlider.maxValue = maxPlayerHealth;
        }

        if (walkingButtonGameObject != null)
        {
            walkingButton = walkingButtonGameObject.GetComponent<Button>();
        }

        if (runningButtonGameObject != null)
        {
            runningButton = runningButtonGameObject.GetComponent<Button>();
        }

        InvokeRepeating(nameof(DoPlayerProgress), 0.1f, 0.1f);
    }

    private void DoPlayerProgress()
    {
        if (playerProgressSlider != null && !gameIsPaused)
        {
            if (Data.walking)
            {
                Data.playerProgress += 0.1f;
            }
            if (Data.running)
            {
                Data.playerProgress += 0.3f;
            }

            playerProgressSlider.value = Data.playerProgress;
        }
    }

    private void Awake()
    {
        if (playerProgressSlider != null && playerHealthSlider != null)
        {
            playerHealthSlider.value = Data.playerHealth;
            playerProgressSlider.value = Data.playerProgress;
        }
    }

    private void takeDamage(int damage)
    {

    }

    public void startWalking()
    {
        stopRunning();
        Data.walking = true;
    }

    public void stopWalking()
    {
        Data.walking = false;
    }

    public void startRunning()
    {
        stopWalking();
        Data.running = true;
    }

    public void stopRunning()
    {
        Data.running = false;
    }
    //TO - DO
    //paus
    //game win/lose
    //fiender
    //random fiende väljare
    //välja klass innan fiende och visa vilka minigames / klass
}
