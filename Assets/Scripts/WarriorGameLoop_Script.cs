using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class WarriorGameLoop_Script : MonoBehaviour
{
    private static float previousProgressMadeInSeconds;
    private static bool ainaSharpshooterPlayed, kajsaCandyCrushPlayed, zakariasBrickPlayed;

    [SerializeField] private GameObject gameProgressSliderGameObject, pausedGamePanel, playerHealthSliderGameObject, walkingButtonGameObject, runningButtonGameObject, gameWinPanelGameObject, gameLosePanelGameObject;
    [SerializeField] private float timeToReachEndInSeconds;
    [SerializeField] private int playerMaxHealth;

    private Slider playerProgressSlider, playerHealthSlider;

    private Button walkingButton, runningButton;

    private bool walking, running, playedEvent1, playedEvent2, playedEvent3, playedEvent4, playedEvent5, playedEvent6, gameWinIsCalled, gameLoseIsCalled, gameProgressIsActive;

    private float playerCurrentHealth;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartTime();

        pausedGamePanel.SetActive(false);
        gameWinPanelGameObject.SetActive(false);
        gameLosePanelGameObject.SetActive(false);


        if (gameProgressSliderGameObject != null)
        {
            playerProgressSlider = gameProgressSliderGameObject.GetComponent<Slider>();
            playerProgressSlider.maxValue = timeToReachEndInSeconds;
            playerProgressSlider.value = previousProgressMadeInSeconds;
        }

        if (playerHealthSliderGameObject != null)
        {
            playerHealthSlider = playerHealthSliderGameObject.GetComponent<Slider>();
            playerHealthSlider.maxValue = playerMaxHealth;
            playerCurrentHealth = playerMaxHealth;
            playerHealthSlider.value = playerCurrentHealth;
        }

        if (walkingButtonGameObject != null)
        {
            walkingButton = walkingButtonGameObject.GetComponent<Button>();
        }

        if (runningButtonGameObject != null)
        {
            runningButton = runningButtonGameObject.GetComponent<Button>();
        }
    }

    private void Update()
    {
        if (playerCurrentHealth <= 0 && !gameLoseIsCalled)
        {
            gameLoseIsCalled = true;
            GameLose();
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Aina_SharpshooterGame"))
        {
            GameObject gameControllerObject = GameObject.Find("Game Controller");
            GameController gameController = gameControllerObject.GetComponent<GameController>();
            if (gameController.gameOver)
            {
                SceneManager.LoadScene(1);
            }
        }

        playerHealthSlider.value = playerCurrentHealth;

        if (playerProgressSlider != null && gameProgressIsActive)
        {
            if (previousProgressMadeInSeconds > 0)
            {
                playerProgressSlider.value = previousProgressMadeInSeconds;
                previousProgressMadeInSeconds = 0;
            }
            if (walking || running)
            {
                playerProgressSlider.value += Time.deltaTime;
            }
        }

        if (playerProgressSlider.value >= 4 && playerProgressSlider.value <= 5 && !playedEvent1)
        {
            playedEvent1 = true;
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 9 && playerProgressSlider.value <= 10 && !playedEvent2)
        {
            playedEvent2 = true;
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 14 && playerProgressSlider.value <= 15 && !playedEvent3)
        {
            playedEvent3 = true;
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 19 && playerProgressSlider.value <= 20 && !playedEvent4)
        {
            playedEvent4 = true;
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 24 && playerProgressSlider.value <= 25 && !playedEvent5)
        {
            playedEvent5 = true;
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 28 && playerProgressSlider.value <= 29 && !playedEvent6)
        {
            playedEvent6 = true;
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 30 && !gameWinIsCalled)
        {
            gameWinIsCalled = true;
            GameWin();
        }
    }

    private void ResetMinigameLimiters()
    {
        ainaSharpshooterPlayed = false;
        kajsaCandyCrushPlayed = false;
        zakariasBrickPlayed = false;
    }

    private void DisableWalkButton()
    {
        walkingButton.interactable = false;
    }

    private void EnableWalkButton()
    {
        walkingButton.interactable = true;
    }

    private void DisableRunButton()
    {
        runningButton.interactable = false;
    }

    private void EnableRunButton()
    {
        runningButton.interactable = true;
    }

    public void ChangeToRun()
    {
        DisableRunButton();
        EnableWalkButton();
        running = true;
        walking = false;
        Time.timeScale = 1.5f;
        ResumeGameProgress();
    }

    public void ChangeToWalk()
    {
        DisableWalkButton();
        EnableRunButton();
        walking = true;
        running = false;
        StartTime();
        ResumeGameProgress();
    }

    private void TakeDamage(int damage)
    {
        playerCurrentHealth = playerCurrentHealth - damage;
    }

    private void GameLose()
    {
        gameLosePanelGameObject.SetActive(true);
        ResetMinigameLimiters();
        Invoke(nameof(LoadMainMenu), 5f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void GameWin()
    {
        ResetMinigameLimiters();
        gameWinPanelGameObject.SetActive(true);
        Invoke(nameof(LoadMainMenu), 5f);
    }

    private void TimedEventHappening()
    {
        PauseGameProgress();
        previousProgressMadeInSeconds = playerProgressSlider.value;
        if (walking)
        {
            GetRandomMinigame();
        }
        if (running)
        {
            GetRandomMinigameOrEvent();
        }
    }

    private void GetRandomMinigameOrEvent()
    {
        //TEMPORARY
        if (!zakariasBrickPlayed)
        {
            zakariasBrickPlayed = true;
            SceneManager.LoadScene(2);
            return;
        }
        //if (!kajsaCandyCrushPlayed)
        //{
        //    kajsaCandyCrushPlayed = true;
        //    SceneManager.LoadScene(6);
        //    return;
        //}
        if (!ainaSharpshooterPlayed)
        {
            ainaSharpshooterPlayed = true;
            SceneManager.LoadScene(3);
            return;
        }
        Random rnd50 = new Random();
        int randomNumber = rnd50.Next(0, 2);
        if (randomNumber == 0)
        {
            if (playerCurrentHealth < playerMaxHealth)
            {
                TakeDamage(-1);
            }
            return;
        }
        if (randomNumber == 1)
        {
            TakeDamage(1);
        }
    }

    private void GetRandomMinigame()
    {
        //TEMPORARY
        if (!zakariasBrickPlayed)
        {
            zakariasBrickPlayed = true;
            SceneManager.LoadScene(2);
            return;
        }
        //if (!kajsaCandyCrushPlayed)
        //{
        //    kajsaCandyCrushPlayed = true;
        //    SceneManager.LoadScene(6);
        //    return;
        //}
        if (!ainaSharpshooterPlayed)
        {
            ainaSharpshooterPlayed = true;
            SceneManager.LoadScene(3);
            return;
        }
        Random rnd25 = new Random();
        int randomNumber = rnd25.Next(0, 4);
        if (randomNumber == 0)
        {
            if (playerCurrentHealth < playerMaxHealth)
            {
                TakeDamage(-1);
            }
            return;
        }
        if (randomNumber >= 0)
        {
            TakeDamage(1);
        }
    }

    public void PauseGame()
    {
        pausedGamePanel.SetActive(true);
        PauseGameProgress();
    }

    public void ResumeGame()
    {
        ResumeGameProgress();
        pausedGamePanel.SetActive(false);
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void StartTime()
    {
        Time.timeScale = 1f;
    }

    private void PauseGameProgress()
    {
        gameProgressIsActive = false;
    }

    private void ResumeGameProgress()
    {
        gameProgressIsActive = true;
    }
}
