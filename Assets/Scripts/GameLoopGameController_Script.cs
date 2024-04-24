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
    [SerializeField] private GameObject enemyOneGameObject;
    [SerializeField] private GameObject enemyTwoGameObject;
    [SerializeField] private GameObject eventOneGameObject;
    [SerializeField] private GameObject eventTwoGameObject;
    [SerializeField] private GameObject itemSwordGameObject;
    [SerializeField] private GameObject itemShieldGameObject;
    [SerializeField] private GameObject bossGameObject;


    private UnityEngine.UI.Slider playerProgressSlider, playerHealthSlider;

    private UnityEngine.UI.Button walkingButton, runningButton, pauseButton;

    private bool gameIsPaused, gameIsWon, gameIsLost;

    private int bossCurrentHealth;

    private int bossMaxHealth = 50;

    //Antal steg till spel loopens slut i walking pace (1/1) vilket blir 60 sekunder
    private int maxGameProgress = 40;
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

        if (Data.playerProgress == maxGameProgress && !gameIsWon) //win on max progress
        {
            gameIsWon = true;
            WinGame();
            Debug.Log("win");
        }

        if (Data.playerHealth <= 0 && !gameIsLost) //lose when health is 0
        {
            gameIsLost = true;
            LoseGame();
            Debug.Log("Launching game in game loop scene results in health = 0 which is game lose");
        }

        if (Data.playerLose) //player loses encounter
        {
            if (Data.activeEventOrEnemy == "ENEMY")
            {
                Data.playerLose = false;
                Data.activeEventOrEnemy = "";
                PlayerLoseInEnemy();
            }
            else if(Data.activeEventOrEnemy == "EVENT")
            {
                Data.playerLose = false;
                Data.activeEventOrEnemy = "";
                PlayerLoseInEvent();
            }

        }

        if (Data.playerWin) //player wins encounter
        {
            if (Data.activeEventOrEnemy == "ENEMY")
            {
                Data.playerWin = false;
                Data.activeEventOrEnemy = "";
                PlayerWinInEnemy();
            }
            else if (Data.activeEventOrEnemy == "EVENT")
            {
                Data.playerWin= false;
                Data.activeEventOrEnemy = "";
                PlayerWinInEvent();
            }
        }

        //check progress

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
        gameIsPaused = true;
    }

    private void WinGame()
    {
        gameWinPanelGameObject.SetActive(true);
        gameIsPaused = true;
    }

    private void GetRandomEventOrEnemy()
    {
        gameIsPaused = true;

        List<GameObject> possibleEncounters = new List<GameObject>();

        if (!Data.enemyOneMet)
        {
            possibleEncounters.Add(enemyOneGameObject);
        }

        if (!Data.enemyTwoMet)
        {
            possibleEncounters.Add(enemyTwoGameObject);
        }

        if (!Data.eventOneMet)
        {
            possibleEncounters.Add(eventOneGameObject);
        }

        if (!Data.eventTwoMet)
        {
            possibleEncounters.Add(eventTwoGameObject);
        }

        if (possibleEncounters.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleEncounters.Count);
            GameObject randomEncounter = possibleEncounters[randomIndex];
            
            if (randomEncounter.Equals(enemyOneGameObject))
            {
                Data.enemyOneMet = true;
                Data.activeEventOrEnemy = "ENEMY";
                enemyOneGameObject.SetActive(true);
            }

            if (randomEncounter.Equals(enemyTwoGameObject))
            {
                Data.enemyTwoMet = true;
                Data.activeEventOrEnemy = "ENEMY";
                enemyTwoGameObject.SetActive(true);
            }

            if (randomEncounter.Equals(eventOneGameObject))
            {
                Data.eventOneMet = true;
                Data.activeEventOrEnemy = "EVENT";
                eventOneGameObject.SetActive(true);
            }

            if (randomEncounter.Equals(eventTwoGameObject))
            {
                Data.eventTwoMet = true;
                Data.activeEventOrEnemy = "EVENT";
                eventTwoGameObject.SetActive(true);
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
            bossCurrentHealth -= damage;
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

    public void LoseViaTemporaryButton()
    {
        Data.playerLose = true;
        DeactivateAllEncounterPanels();
        gameIsPaused = false;
    }

    public void WinViaTemporaryButton()
    {
        Data.playerWin = true;
        DeactivateAllEncounterPanels();
        gameIsPaused = false;
    }

    private void DeactivateAllEncounterPanels()
    {
        enemyOneGameObject.SetActive(false);
        enemyTwoGameObject.SetActive(false);
        eventOneGameObject.SetActive(false);
        eventTwoGameObject.SetActive(false);
    }













    //TO - DO
    //2 fiender som är kopplat till enstaka minigames --> kan ta skada eller förlora item
    //2 event tvingat specifikt minigame --> kan få item, hälsa eller förlora hälsa eller ingenting händer
    //random fiende och event väljare i game loopen
    //Items som påverkar boss fight
    //Vid förlust mot vanlig fiende kan man förlora item
    //Boss --> 3 olika spel för bossen
    //Fiender med olika attribut (vissa gör mer skada andra andra har större chans att sno item ex drake vs goblin)
}
