using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static float playerProgress;
    public static float playerHealth;
    public static float bossHealth;

    public static string activeEventOrEnemy; //Skickas in som 'EVENT' eller 'ENEMY' eller 'BOSS'

    public static bool walking, running;
    public static bool hasItemSword, hasItemSheild;
    public static bool enemyOneMet, enemyTwoMet;
    public static bool eventOneMet, eventTwoMet;
    public static bool playerWin, playerLose;
    public static bool encounterOneMet, encounterTwoMet;
    public static bool encounterThreeMet, encounterFourMet;
    public static bool encounterOneComplete, encounterTwoComplete;
    public static bool encounterThreeComplete, encounterFourComplete;
    public static bool bossBattleIsActive;

    public static int currentActiveMinigame;

    public static void Load() //man ska kunna komma tillbaka till gameloop scenen och ett minigame eller event om man l�mnade mitt i det
    {

    }

    public static void Reset() //�terst�ller all sparad data
    {
        playerProgress = 0;
        playerHealth = GameLoopGameController_Script.maxPlayerHealth;
        bossHealth = GameLoopGameController_Script.bossMaxHealth;
        walking = false;
        running = false;
        enemyOneMet = false;
        enemyTwoMet = false;
        eventOneMet = false;
        eventTwoMet = false;
        encounterOneMet = false;
        encounterTwoMet = false;
        encounterThreeMet = false;
        encounterFourMet = false;
        hasItemSheild = false;
        hasItemSword = false;
        bossBattleIsActive = false;
        activeEventOrEnemy = "";
    }

    public static void SaveToFile() //f�r att skriva till .json
    {

    }
}
