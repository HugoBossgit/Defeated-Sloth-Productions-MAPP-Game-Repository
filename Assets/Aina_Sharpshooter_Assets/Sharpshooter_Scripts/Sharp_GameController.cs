using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class GameController : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private float multiplier = 1.0f;
    [SerializeField] private int lives = 3;
    [SerializeField] private string difficulty;
    private bool odd;

    [SerializeField] private TMP_Text pointsText, comboText;
    [SerializeField] private AudioSource audSource;
    [SerializeField] private AudioClip playMusic, waitMusic;
    [SerializeField] private AudioClip[] batDeath, bossDeath, hunterDeath, hunterHurt, ogreDeath, warriorDeath;
    [SerializeField] private GameObject lossBalloon;
    [SerializeField] private GameObject winBalloon;
    [SerializeField] private GameObject readyUpUI;
    [SerializeField] private List<GameObject> hearts = new List<GameObject>();
    public GameObject infoBalloon;
    [SerializeField] private GameObject goblin, armGoblin, bigGoblin, boss;
    [SerializeField] private HunterBehavior hunter;
    
    public bool gameOver, gameWon;
    private bool ready;
    private int comboStarter;
    private int deleted;
    private int enemyNum;
    private int spawned;
    private int goblinSP, armGoblinSP, bigGoblinSP;
    private int gobToSp, armToSp, bigToSp;
    private bool bossSpawned;

    void Start()
    {
        audSource = GetComponent<AudioSource>();
        lossBalloon.SetActive(false);
        winBalloon.SetActive(false);
        readyUpUI.SetActive(true);
        infoBalloon.SetActive(false);
        difficulty = "Boss";
        audSource.PlayOneShot(waitMusic);

        if(difficulty.Equals("Medium"))
        {
            enemyNum = 20;
            gobToSp = enemyNum / 2 + (int)UnityEngine.Random.Range(0, enemyNum/3);
            armToSp = enemyNum - gobToSp - 1;
            bigToSp = 1;
        }
        else if(difficulty.Equals("Hard"))
        {
            enemyNum = 30;
            gobToSp = enemyNum / 2 + (int)UnityEngine.Random.Range(0, enemyNum / 3);
            armToSp = enemyNum - gobToSp - 3;
            bigToSp = 2;
        }
        else if(difficulty.Equals("Boss"))
        {
            enemyNum = 30;
            gobToSp = enemyNum / 2 + (int)UnityEngine.Random.Range(0, enemyNum / 4);
            armToSp = enemyNum - gobToSp - 4;
            bigToSp = 3;
            infoBalloon.SetActive(true);
        }
        else
        {
            infoBalloon.SetActive(true);
            enemyNum = 10;
            gobToSp = enemyNum / 2 + (int)UnityEngine.Random.Range(0, enemyNum / 3);
            armToSp = enemyNum - gobToSp;
        }

        for (int i = 0; i < enemyNum / 2; i++)
        {
            if (difficulty.Equals("Medium"))
            { spawnGoblin(); }
            if (difficulty.Equals("Hard"))
            { spawnGoblin(); }
            if (difficulty.Equals("Boss"))
            { spawnGoblin(); }
            else
            { spawnGoblin(); }
        }

    }

    void FixedUpdate()
    {
        audSource.volume = 1.5f;
        if (enemyNum == deleted && !gameOver)
        {
            Win();
        }

        if (ready && !gameOver) 
        {
            if(difficulty.Equals("Medium"))
            {
                if(goblinSP < gobToSp) 
                {
                    Invoke("spawnGoblin", 1f);
                }

                if(armGoblinSP < armToSp)
                {
                    Invoke("spawnArmGoblin", 2f);
                }

                if(goblinSP == gobToSp && armGoblinSP == armToSp && bigGoblinSP < bigToSp)
                {
                    Invoke("spawnBigGoblin", 5f);
                }

            }
            else if(difficulty.Equals("Hard"))
            {
                if (goblinSP < gobToSp)
                {
                    Invoke("spawnGoblin", 1f);
                }

                if (armGoblinSP < armToSp)
                {
                    Invoke("spawnArmGoblin", 2f);
                }

                if (goblinSP == gobToSp && armGoblinSP == armToSp && bigGoblinSP < bigToSp)
                {
                    Invoke("spawnBigGoblin", 5f);
                }
            }
            else if(difficulty.Equals("Boss"))
            {
                if (goblinSP < gobToSp)
                {
                    Invoke("spawnGoblin", 1f);
                }

                if (armGoblinSP < armToSp)
                {
                    Invoke("spawnArmGoblin", 2f);
                }

                if (goblinSP == gobToSp && armGoblinSP == armToSp && bigGoblinSP < bigToSp)
                {
                    Invoke("spawnBigGoblin", 5f);
                }

                if(goblinSP == gobToSp && armGoblinSP == armToSp && bigGoblinSP == bigToSp && !bossSpawned)
                {
                    Invoke("spawnBoss", 10f);
                }
            }
            else
            {
                if (goblinSP < gobToSp)
                {
                    Invoke("spawnGoblin", 1f);
                }

                if (armGoblinSP < armToSp)
                {
                    Invoke("spawnArmGoblin", 2f);
                }
            }

        }
    }

    public void addPoints(int value)
    {
        points += (int)(value * multiplier);
        pointsText.text = points.ToString();
    }

    public void combo(Boolean increase)
    {
        comboStarter++;
        if (increase && comboStarter > 2) 
        {
            multiplier += 0.1f;
        }
        
        if(!increase)
        {
            multiplier = 1.0f;
        }

        comboText.text = multiplier.ToString();
    }

    public void DecrementLives(int decrease)
    {
        lives -= decrease;
        hunter.hurt();
        makeNoise("HunterHurt");
        if (lives >= 0)
        {
            Destroy(hearts.ElementAt(lives));
            //hearts.RemoveAt(lives);
        }
        
        if(lives <= 0)
        {
            makeNoise("HunterDeath");
            Loss();
        }
    }

    public void makeNoise(String type)
    {

        odd = !odd;
        if (type.Equals("Bat") && odd)
        {
            
            int clipIndex = UnityEngine.Random.Range(1, batDeath.Length);
            AudioClip clip = batDeath[clipIndex];
            audSource.PlayOneShot(clip);
            batDeath[clipIndex] = batDeath[0];
            batDeath[0] = clip;
        }
        else if(type.Equals("Warrior") && odd)
        {
            int clipIndex = UnityEngine.Random.Range(1, warriorDeath.Length);
            AudioClip clip = warriorDeath[clipIndex];
            audSource.PlayOneShot(clip);
            warriorDeath[clipIndex] = warriorDeath[0];
            warriorDeath[0] = clip;
        }
        else if(type.Equals("Ogre"))
        {
            int clipIndex = UnityEngine.Random.Range(1, ogreDeath.Length);
            AudioClip clip = ogreDeath[clipIndex];
            audSource.PlayOneShot(clip);
            ogreDeath[clipIndex] = ogreDeath[0];
            ogreDeath[0] = clip;
        }
        else if(type.Equals("Boss"))
        {
            int clipIndex = UnityEngine.Random.Range(0, bossDeath.Length);
            AudioClip clip = bossDeath[clipIndex];
            audSource.PlayOneShot(clip);
        }
        else if(type.Equals("HunterHurt"))
        {
            int clipIndex = UnityEngine.Random.Range(1, hunterHurt.Length);
            AudioClip clip = hunterHurt[clipIndex];
            audSource.PlayOneShot(clip);
            hunterHurt[clipIndex] = hunterHurt[0];
            hunterHurt[0] = clip;
        }
        else if(type.Equals("HunterDeath"))
        {
            int clipIndex = UnityEngine.Random.Range(0, hunterDeath.Length);
            audSource.PlayOneShot(hunterDeath[clipIndex]);
        }
    }

    public void Loss()
    {
        gameOver = true;
        lossBalloon.SetActive(true);
        Invoke("declareLoss", 4f);
    }

    public void Win()
    {
        gameWon = true;
        winBalloon.SetActive(true);
        Invoke("declareWin", 6f);
    }

    public void Restart()
    { }

    public void startGame()
    {
        audSource.Stop();
        readyUpUI.SetActive(false);
        ready = true;
        audSource.PlayOneShot(playMusic);
    }

    public bool getReady()
    { 
        return ready; 
    }

    public void addDeleted()
    {
        deleted++;
    }

    private void spawnGoblin()
    {

        Debug.Log("vanliga " + goblinSP);
        if (spawned < enemyNum && goblinSP < gobToSp)
        {
            spawned++;
            goblinSP++;
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(1.5f, 3));
            Instantiate(goblin, spawnArea, Quaternion.identity);
        }
    }

    private void spawnArmGoblin()
    {

        Debug.Log("ovanliga " + armGoblinSP);
        if (spawned < enemyNum && armGoblinSP < armToSp)
        {
            spawned++;
            armGoblinSP++;
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(1.5f, 3));
            Instantiate(armGoblin, spawnArea, Quaternion.identity);
        }
    }

    private void spawnBigGoblin()
    {

        if (spawned < enemyNum && bigGoblinSP < bigToSp)
        {
            spawned++;
            bigGoblinSP++;
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(1.5f, 3));
            Instantiate(bigGoblin, spawnArea, Quaternion.identity);
        }
    }

    private void spawnBoss()
    {

        if (spawned < enemyNum)
        {
            spawned++;
            bossSpawned = true;
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(1.5f, 3));
        Instantiate(boss, spawnArea, Quaternion.identity);
        }
    }

    public void declareWin()
    {
        Data.playerWin = true;
        SceneManager.LoadScene(1);
    }

    public void declareLoss()
    {
        Data.playerLose = true;
        SceneManager.LoadScene(1);
    }

    public void declareDifficulty(String s)
    {
        difficulty = s;
    }

}
