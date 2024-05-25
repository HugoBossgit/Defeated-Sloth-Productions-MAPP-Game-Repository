using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLoopGameController_Script : MonoBehaviour 
    //OM MAN SKA L�GGA TILL FLER ENOUNTERS I GAME LOOP F�LJ ALLA RELEVANTA KOMMENTARER!
    //GL�M INTE ATT L�GGA TILL NYA VARIABLER I DATA OCKS�!
{
    [SerializeField] private GameObject gameProgressSliderGameObject;
    [SerializeField] private GameObject pausedGamePanel;
    [SerializeField] private GameObject playerHealthSliderGameObject;
    [SerializeField] private GameObject walkingButtonGameObject;
    [SerializeField] private GameObject runningButtonGameObject;
    [SerializeField] private GameObject pauseButtonGameObject;
    [SerializeField] private GameObject gameWinPanelGameObject;
    [SerializeField] private GameObject gameLosePanelGameObject;
    [SerializeField] private GameObject itemSwordGameObject;
    [SerializeField] private GameObject itemShieldGameObject;
    [SerializeField] private GameObject bossGameObject;
    [SerializeField] private GameObject bossHealtSliderGameObject;
    [SerializeField] private GameObject battleBossButtonGameObject;
    [SerializeField] private GameObject SceneTransitionObject;
    [SerializeField] private ParticleSystem swordGetPArticleEffect;
    [SerializeField] private ParticleSystem shieldGetPArticleEffect;
    [SerializeField] private AudioSource audioSourceItemGet;
    [SerializeField] private int enemyOne, enemyTwo, eventOne, eventTwo, bossOne, bossTwo;


    private UnityEngine.UI.Slider playerProgressSlider, playerHealthSlider, bossHealthSlider;

    private UnityEngine.UI.Button walkingButton, runningButton, pauseButton;

    private bool gameIsPaused, gameIsWon, gameIsLost, bossMinigamePlaying;

    private Animator sceneTransitionAnimator;

    private AudioSource backgroundMusic;

    public const int bossMaxHealth = 50;

    //Antal steg till spel loopens slut i walking pace (1/1) vilket blir 60 sekunder
    private int maxGameProgress = 50;
    public const int maxPlayerHealth = 10;

    private void Awake()
    {
        if (playerProgressSlider != null && playerHealthSlider != null)
        {
            playerHealthSlider.value = Data.playerHealth;
            playerProgressSlider.value = Data.playerProgress;
        }

        Data.currentActiveMinigame = 0;
    }

    private void Start()
    {
        sceneTransitionAnimator = SceneTransitionObject.GetComponent<Animator>();
        pausedGamePanel.SetActive(false);
        gameWinPanelGameObject.SetActive(false);
        gameLosePanelGameObject.SetActive(false);
        bossGameObject.SetActive(false);
        battleBossButtonGameObject.SetActive(false);
        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;

        Data.encounterOneComplete = false;
        Data.encounterTwoComplete = false;
        Data.encounterThreeComplete = false;
        Data.encounterFourComplete = false;

        if (bossHealtSliderGameObject != null)
        {
            bossHealthSlider = bossHealtSliderGameObject.GetComponent<UnityEngine.UI.Slider>();
            bossHealthSlider.maxValue = bossMaxHealth;
        }

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

        InvokeRepeating(nameof(DoPlayerProgress), 0, 1);
    }

    private IEnumerator FadeOut()
    {
        sceneTransitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(0);
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

    private void DoPlayerProgress()
    {
        if(playerProgressSlider != null && !gameIsPaused && !Data.returningToMainMenu)
        {
            if(Data.walking)
            {
                Data.playerProgress += 2;
            }
            if(Data.running)
            {
                Data.playerProgress += 4;
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
        if (!gameIsPaused && !Data.returningToMainMenu)
        {
            UpdateWalkAndRunButtons();
        }
        playerProgressSlider.value = Data.playerProgress;
        playerHealthSlider.value = Data.playerHealth;

        if(Data.bossBattleIsActive)
        {
            gameIsPaused = true;
            bossGameObject.SetActive(true);
            battleBossButtonGameObject.SetActive(true);
            runningButtonGameObject.SetActive(false);
            walkingButtonGameObject.SetActive(false);
            bossHealthSlider.value = Data.bossHealth;
            Data.activeEventOrEnemy = "BOSS";

            if (Data.bossHealth <= 0)
            {
                WinGame();
            }
        }

        if (Data.currentActiveMinigame != 0) //M�ste Expanderas! om man l�mnar medans minigame �r aktivt ska minigame b�rja om
        {
            if (Data.currentActiveMinigame == 1 && !Data.enemyOneMet) //enemy one
            {
                SceneManager.LoadScene(enemyOne);
            }

            if (Data.currentActiveMinigame == 2 && !Data.enemyTwoMet) //enemy two
            {
                SceneManager.LoadScene(enemyTwo);
            }

            if (Data.currentActiveMinigame == 3 && !Data.eventOneMet) //event one
            {
                SceneManager.LoadScene(eventOne);
            }

            if (Data.currentActiveMinigame == 3 && !Data.eventTwoMet) //event two
            {
                SceneManager.LoadScene(eventTwo);
            }
        }

        if (Data.hasItemSheild)
        {
            itemShieldGameObject.SetActive(true);
        }
        else
        {
            itemShieldGameObject.SetActive(false);
        }

        if (Data.hasItemSword)
        {
            itemSwordGameObject.SetActive(true);
        }
        else
        {
            itemSwordGameObject.SetActive(false);
        }

        if (Data.playerProgress == maxGameProgress && !gameIsWon) //win on max progress / after boss
        {
            gameIsWon = true;
            WinGame();
            Debug.Log("Win");
        }

        if (Data.playerHealth <= 0 && !gameIsLost) //lose when health is 0
        {
            gameIsLost = true;
            LoseGame();
            Debug.Log("Lose");
        }

        if (Data.playerLose) //player loses encounter
        {
            int randomIndex = UnityEngine.Random.Range(0, 2); //justera chans att f�rlora item

            if (Data.activeEventOrEnemy == "ENEMY")
            {
                Data.playerLose = false;
                Data.activeEventOrEnemy = "";
                if (randomIndex == 0)
                {
                    Data.hasItemSword = false;
                }
                PlayerLoseInEnemy();
            }
            else if(Data.activeEventOrEnemy == "EVENT")
            {
                Data.playerLose = false;
                Data.activeEventOrEnemy = "";
                if (randomIndex == 0)
                {
                    Data.hasItemSword = false;
                }
                PlayerLoseInEvent();
            }
            else if (Data.activeEventOrEnemy == "BOSS")
            {
                Data.playerLose = false;
                Data.activeEventOrEnemy = "";
                LoseRoundAgainstBoss();
            }

        }

        if (Data.playerWin) //player wins encounter
        {
            int randomIndex = UnityEngine.Random.Range(0, 2); //justera chans att f� item

            if (Data.activeEventOrEnemy == "ENEMY")
            {
                Data.playerWin = false;
                Data.activeEventOrEnemy = "";
                if (randomIndex == 1)
                {
                    Data.hasItemSword = true;
                    StartCoroutine(TriggerParticleEffectSwordWithDelay(3f));
                }
                PlayerWinInEnemy();
            }
            else if (Data.activeEventOrEnemy == "EVENT")
            {
                Data.playerWin = false;
                Data.activeEventOrEnemy = "";
                if (randomIndex == 1)
                {
                    Data.hasItemSheild = true;
                    StartCoroutine(TriggerParticleEffectShieldWithDelay(3f));
                }
                PlayerWinInEvent();
            }
            else if (Data.activeEventOrEnemy == "BOSS")
            {
                Data.playerWin = false;
                Data.activeEventOrEnemy = "";
                WinRoundAgainstBoss();
            }
        }

        //check progress //M�ste Expanderas! add more ecounters and make running player skip them while walking hits all

        if (Data.playerProgress > 5 && !Data.encounterOneMet)
        {
            Data.encounterOneMet = true;
            GetRandomEventOrEnemy();
        }

        if (Data.playerProgress > 15 && !Data.encounterTwoMet)
        {
            Data.encounterTwoMet = true;
            GetRandomEventOrEnemy();
        }

        if (Data.playerProgress > 25 && !Data.encounterThreeMet)
        {
            Data.encounterThreeMet = true;
            GetRandomEventOrEnemy();
        }

        if (Data.playerProgress > 35 && !Data.encounterFourMet)
        {
            Data.encounterFourMet = true;
            GetRandomEventOrEnemy();
        }

        if (Data.playerProgress > 40 && !Data.bossBattleIsActive && !gameIsLost && !gameIsWon)
        {
            Data.bossBattleIsActive = true;
            BossBattle();
        }
    }

    private IEnumerator TriggerParticleEffectSwordWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        swordGetPArticleEffect.Play();
        audioSourceItemGet.Play();


    }

    private IEnumerator TriggerParticleEffectShieldWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shieldGetPArticleEffect.Play();
        audioSourceItemGet.Play();
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
        bossGameObject.SetActive(false);
        InactivateButtons();
    }

    public void UnPauseGame()
    {
        ActivateButtons();
        pausedGamePanel.SetActive(false);
        gameIsPaused = false;
        if(Data.bossBattleIsActive)
        {
            bossGameObject.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        Data.returningToMainMenu = true;
        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;
        ActivateButtons();
        StartCoroutine(FadeOut());
    }

    public void ReturnToMainMenuAndReset()
    {
        Data.returningToMainMenu = true;
        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;
        ActivateButtons();
        Data.Reset();
        StartCoroutine(FadeOut());
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

    private void UpdateWalkAndRunButtons()
    {
        if (Data.walking)
        {
            walkingButton.Select();
            return;
        }

        if(Data.running)
        {
            runningButton.Select(); 
            return;
        }
    }

    private void LoseGame()
    {
        gameLosePanelGameObject.SetActive(true);
        bossGameObject.SetActive(false);
        gameIsPaused = true;
    }

    private void WinGame()
    {
        Data.playerProgress = maxGameProgress;
        gameWinPanelGameObject.SetActive(true);
        bossGameObject.SetActive(false);
        gameIsPaused = true;
    }

    private void GetRandomEventOrEnemy() //M�ste Expanderas!
    {
        gameIsPaused = true;

        List<int> possibleEncounters = new List<int>();

        if (!Data.enemyOneMet)
        {
            possibleEncounters.Add(enemyOne);
        }

        if (!Data.enemyTwoMet)
        {
            possibleEncounters.Add(enemyTwo);
        }

        if (!Data.eventOneMet)
        {
            possibleEncounters.Add(eventOne);
        }

        if (!Data.eventTwoMet)
        {
            possibleEncounters.Add(eventTwo);
        }

        if (possibleEncounters.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleEncounters.Count);
            int randomEncounter = possibleEncounters[randomIndex];
            
            if (randomEncounter.Equals(enemyOne))
            {
                Data.currentActiveMinigame = 1; //expandera med nya encounters! Data.currentActiveMinigame = n;
                Data.enemyOneMet = true;
                Data.activeEventOrEnemy = "ENEMY";
                SceneManager.LoadScene(enemyOne);
                return;
            }

            if (randomEncounter.Equals(enemyTwo))
            {
                Data.currentActiveMinigame = 2;
                Data.enemyTwoMet = true;
                Data.activeEventOrEnemy = "ENEMY";
                SceneManager.LoadScene(enemyTwo);
                return;
            }

            if (randomEncounter.Equals(eventOne))
            {
                Data.currentActiveMinigame = 3;
                Data.eventOneMet = true;
                Data.activeEventOrEnemy = "EVENT";
                SceneManager.LoadScene(eventOne);
                return;
            }

            if (randomEncounter.Equals(eventTwo))
            {
                Data.currentActiveMinigame = 4;
                Data.eventTwoMet = true;
                Data.activeEventOrEnemy = "EVENT";
                SceneManager.LoadScene(eventTwo);
                return;
            }
        }
        else
        {
            Debug.Log("NoEncountersLeft");
        }
    }

    private void PlayerWinInEnemy()
    {
        RecoverHealth(2);
    }

    private void PlayerWinInEvent()
    {
        RecoverHealth(5);
    }
    private void PlayerLoseInEnemy()
    {
        TakeDamage(5, "PLAYER");
    }

    private void PlayerLoseInEvent()
    {
        TakeDamage(2, "PLAYER");
    }

    private void TakeDamage(int damage, String bossOrPlayer)
    {
        if (bossOrPlayer == "PLAYER")
        {
            Data.playerHealth -= damage;
        }

        if (bossOrPlayer == "BOSS")
        {
            Data.bossHealth -= damage;
        }
    }

    private void RecoverHealth(int healthToRecover)
    {
        Data.playerHealth += healthToRecover;

        if (Data.playerHealth > maxPlayerHealth)
        {
            Data.playerHealth = maxPlayerHealth;
        }
    }
    
    private void BossBattle()
    {
        gameIsPaused = true;
        bossGameObject.SetActive(true);
        battleBossButtonGameObject.SetActive(true);
        walkingButtonGameObject.SetActive(false);
        runningButtonGameObject.SetActive(false);
    }

    public void BattleBossButton()
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);
        
        if (randomIndex == 0)
        {
            SceneManager.LoadScene(bossOne);
        }

        if (randomIndex == 1)
        {
            SceneManager.LoadScene(bossTwo);
        }

        if (randomIndex == 2) {
            SceneManager.LoadScene("Aina_SharpshooterGame");
        }
    }

    private void StartBossMinigame() //random 3 minispel
    {
        bossMinigamePlaying = true;
    }

    private void LoseRoundAgainstBoss() //justera skada och eller h�lsa
    {
        if (Data.hasItemSheild)
        {
            TakeDamage(1, "PLAYER");
        }
        else
        {
            TakeDamage(3, "PLAYER");
        }
    }

    private void WinRoundAgainstBoss() //justera skada och eller h�lsa
    {
        if (Data.hasItemSword)
        {
            TakeDamage(15, "BOSS");
        }else
        {
            TakeDamage(10, "BOSS");
        }
    }

    public void LoseAgainstBossButton()
    {
        Data.playerLose = true;
    }

    public void WinAgainstBossButton()
    {
        Data.playerWin = true;
    }
}
