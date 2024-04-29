using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.LowLevel;
using System.Xml;
using System.Security.Cryptography;

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
    [SerializeField] private int enemyOne, enemyTwo, eventOne, eventTwo, bossOne, bossTwo;


    private UnityEngine.UI.Slider playerProgressSlider, playerHealthSlider, bossHealthSlider;

    private UnityEngine.UI.Button walkingButton, runningButton, pauseButton;

    private bool gameIsPaused, gameIsWon, gameIsLost, bossMinigamePlaying;

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

    private void DoPlayerProgress()
    {
        if(playerProgressSlider != null && !gameIsPaused)
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
        UpdateButtonColors();
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
        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;
        ActivateButtons();
        SceneManager.LoadScene(0);
    }

    public void ReturnToMainMenuAndReset()
    {
        gameIsPaused = false;
        gameIsWon = false;
        gameIsLost = false;
        ActivateButtons();
        Data.Reset();
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

    private void UpdateButtonColors() //makes buttons appear in correct color when coming back mid game, wont need later, feedback is for my debugging
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






    //TO - DO
    //2 fiender som �r kopplat till enstaka minigames --> kan ta skada eller f�rlora item
    //2 event tvingat specifikt minigame --> kan f� item, h�lsa eller f�rlora h�lsa eller ingenting h�nder
    //random fiende och event v�ljare i game loopen
    //Items som p�verkar boss fight
    //Vid f�rlust mot vanlig fiende kan man f�rlora item
    //Boss --> 3 olika spel f�r bossen
    //Fiender med olika attribut (vissa g�r mer skada andra andra har st�rre chans att sno item ex drake vs goblin)
}
