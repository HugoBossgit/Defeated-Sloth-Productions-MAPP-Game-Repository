using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static float playerProgress;
    public static float playerHealth;
    public static int bossCurrentHealth;

    public static string activeEventOrEnemy; //Skickas in som 'EVENT' eller 'ENEMY'

    public static bool walking, running;
    public static bool hasItemSword, hasItemSheild;
    public static bool enemyOneMet, enemyTwoMet;
    public static bool eventOneMet, eventTwoMet;
    public static bool playerWin, playerLose;
    public static bool encounterOneMet, encounterTwoMet;
    public static bool encounterThreeMet, encounterFourMet;

    public static int currentActiveMinigame;

    public static void Load() //man ska kunna komma tillbaka till gameloop scenen och ett minigame eller event om man lämnade mitt i det
    {

    }

    public static void Reset() //återställer all sparad data
    {
        playerProgress = 0;
        playerHealth = GameLoopGameController_Script.maxPlayerHealth;
        bossCurrentHealth = GameLoopGameController_Script.bossMaxHealth;
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
        activeEventOrEnemy = "";
    }

    public static void SaveToFile() //för att skriva till .json
    {

    }
}
