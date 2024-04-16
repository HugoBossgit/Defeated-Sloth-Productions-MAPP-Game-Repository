using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WarriorGameLoop_Script : MonoBehaviour
{
    [SerializeField] private GameObject gameProgressPanelGameObject, pausedGamePanel, playerHealthSliderGameObject;
    [SerializeField] private float timeToReachEnd, timeBeforeMinigame;
    [SerializeField] private int playerMaxHealth;

    private Slider playerProgressSlider, playerHealthSlider;

    private bool walking, running;

    private float playerCurrentHealth;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartTime();

        pausedGamePanel.SetActive(false);

        if (gameProgressPanelGameObject != null)
        {
            playerProgressSlider = gameProgressPanelGameObject.GetComponent<Slider>();
            playerProgressSlider.maxValue = timeToReachEnd;
        }

        if (playerHealthSliderGameObject != null)
        {
            playerHealthSlider = playerHealthSliderGameObject.GetComponent<Slider>();
            playerHealthSlider.maxValue = playerMaxHealth;
            playerCurrentHealth = playerMaxHealth;
            playerHealthSlider.value = playerCurrentHealth;
        }
    }

    private void Update()
    {
        if (playerCurrentHealth <= 0)
        {
            GameLose();
        }

        playerHealthSlider.value = playerCurrentHealth;

        if (playerProgressSlider != null)
        {
            float playerProgressValue = Mathf.Clamp((Time.timeSinceLevelLoad), 0f, timeToReachEnd);
            playerProgressSlider.value = playerProgressValue;
        }

        if (playerProgressSlider.value >= 4 && playerProgressSlider.value <= 5)
        {
            TakeDamage(1);
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 9 && playerProgressSlider.value <= 10)
        {
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 14 && playerProgressSlider.value <= 15)
        {
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 19 && playerProgressSlider.value <= 20)
        {
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 24 && playerProgressSlider.value <= 25)
        {
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 28 && playerProgressSlider.value <= 29)
        {
            TimedEventHappening();
        }

        if (playerProgressSlider.value >= 30)
        {
            GameWin();
        }
    }

    private void TakeDamage(int damage)
    {
        playerCurrentHealth = playerCurrentHealth - damage;
    }

    private void GameLose()
    {
        Invoke(nameof(LoadMainMenu), 5f);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void GameWin()
    {
        throw new NotImplementedException();
    }

    private void TimedEventHappening()
    {
        StopTime();

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
        throw new NotImplementedException();
    }

    private void GetRandomMinigame()
    {
        throw new NotImplementedException();
    }

    public void PauseGame()
    {
        StopTime();
        pausedGamePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        StartTime();
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
}
