using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameLoopGameController_Script : MonoBehaviour
{
    [SerializeField] private GameObject gameProgressSliderGameObject;
    [SerializeField] private GameObject pausedGamePanel;
    [SerializeField] private GameObject playerHealthSliderGameObject;
    [SerializeField] private GameObject walkingButtonGameObject;
    [SerializeField] private GameObject runningButtonGameObject;
    [SerializeField] private GameObject pauseButtonGameObject;
    [SerializeField] private GameObject gameWinPanelGameObject;
    [SerializeField] private GameObject gameLosePanelGameObject;
    [SerializeField] private List<GameObject> enemies;

    private UnityEngine.UI.Slider playerProgressSlider, playerHealthSlider;

    private UnityEngine.UI.Button walkingButton, runningButton, pauseButton;

    private bool gameIsPaused, gameIsWon, gameIsLost;

    //Antal steg till spel loopens slut i walking pace (1/1) vilket blir 60 sekunder
    private int maxGameProgress = 60;
    public const int maxPlayerHealth = 10;

    private void Awake()
    {
        if (playerProgressSlider != null && playerHealthSlider != null)
        {
            playerHealthSlider.value = Data.playerHealth;
            playerProgressSlider.value = Data.playerProgress;
        }
    }

    private void Start()
    {
        pausedGamePanel.SetActive(false);
        gameWinPanelGameObject.SetActive(false);
        gameLosePanelGameObject.SetActive(false);

        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;


        if(gameProgressSliderGameObject != null)
        {
            playerProgressSlider = gameProgressSliderGameObject.GetComponent<UnityEngine.UI.Slider>();
            playerProgressSlider.maxValue = maxGameProgress;
        }

        if(playerHealthSliderGameObject != null)
        {
            playerHealthSlider = playerHealthSliderGameObject.GetComponent<UnityEngine.UI.Slider>();
            playerHealthSlider.maxValue = maxPlayerHealth;
        }

        if(walkingButtonGameObject != null)
        {
            walkingButton = walkingButtonGameObject.GetComponent<UnityEngine.UI.Button>();
        }

        if(runningButtonGameObject != null)
        {
            runningButton = runningButtonGameObject.GetComponent<UnityEngine.UI.Button>();
        }

        if(pauseButtonGameObject != null)
        {
            pauseButton = pauseButtonGameObject.GetComponent<UnityEngine.UI.Button>();
        }

        InvokeRepeating(nameof(DoPlayerProgress), 1, 1);
    }

    private void DoPlayerProgress()
    {
        if(playerProgressSlider != null && !gameIsPaused)
        {
            if(Data.walking)
            {
                Data.playerProgress += 1;
            }
            if(Data.running)
            {
                Data.playerProgress += 3;
            }

            playerProgressSlider.value = Data.playerProgress;
        }
    }

    private void OnEnable()
    {
        bool healthSliderFound = false;
        while (!healthSliderFound)
        {
            if (GameObject.Find("PlayerHealth_Slider") != null)
            {
                playerHealthSliderGameObject = GameObject.Find("PlayerHealth_Slider");
                playerHealthSlider = playerHealthSliderGameObject.GetComponent<UnityEngine.UI.Slider>();
                playerHealthSlider.value = Data.playerHealth;
                healthSliderFound = true;
            }
        }
        
    }

    private void Update()
    {
        UpdateButtonColors();
        playerProgressSlider.value = Data.playerProgress;

        if (Data.playerProgress == maxGameProgress && !gameIsWon)
        {
            gameIsWon = true;
            WinGame();
            Debug.Log("win");
        }

        if (Data.playerHealth <= 0 && !gameIsLost)
        {
            gameIsLost = true;
            LoseGame();
            Debug.Log("Launching game in game loop scene results in health = 0 which is game lose");
        }
    }

    public void StartWalking()
    {
        StopRunning();
        Data.walking = true;
    }

    private void StopWalking()
    {
        Data.walking = false;
    }

    public void StartRunning()
    {
        StopWalking();
        Data.running = true;
    }

    private void StopRunning()
    {
        Data.running = false;
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        pausedGamePanel.SetActive(true);
        InactivateButtons();
    }

    public void UnPauseGame()
    {
        ActivateButtons();
        pausedGamePanel.SetActive(false);
        gameIsPaused = false;
        UpdateButtonColors();
    }

    public void ReturnToMainMenu()
    {
        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;
        ActivateButtons();
        SceneManager.LoadScene(0);
    }

    private void InactivateButtons()
    {
        runningButton.interactable = false;
        walkingButton.interactable = false;
        pauseButton.interactable = false;
    }

    private void ActivateButtons()
    {
        runningButton.interactable = true;
        walkingButton.interactable = true;
        pauseButton.interactable = true;
    }

    private void UpdateButtonColors()
    {
        ColorBlock walkingButtonColors = walkingButton.colors;
        ColorBlock runningButtonColors = runningButton.colors;

        if (Data.walking)
        {
            walkingButtonColors.normalColor = Color.green;
            runningButtonColors.normalColor = Color.red;

            walkingButton.colors = walkingButtonColors;
            runningButton.colors = runningButtonColors;
            return;
        }

        if(Data.running)
        {
            walkingButtonColors.normalColor = Color.red;
            runningButtonColors.normalColor = Color.green;

            walkingButton.colors = walkingButtonColors;
            runningButton.colors = runningButtonColors;
            return;
        }

        walkingButtonColors.normalColor = Color.white;
        runningButtonColors.normalColor = Color.white;

        walkingButton.colors = walkingButtonColors;
        runningButton.colors = runningButtonColors;
    }

    private void LoseGame()
    {
        gameLosePanelGameObject.SetActive(true);
        gameIsPaused = true;
        Data.Reset();
    }

    private void WinGame()
    {
        gameWinPanelGameObject.SetActive(true);
        gameIsPaused = true;
        Data.Reset();
    }
    //TO - DO
    //fiender
    //random fiende väljare
    //välja klass innan fiende och visa vilka minigames / klass
    //Items
    //Boss
}
