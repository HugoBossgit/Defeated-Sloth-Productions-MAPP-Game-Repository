using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static float playerProgress;
    public static float playerHealth;
    
    public static bool walking;
    public static bool running;

    public static int activeEnemyPointer;

    public static void Load() {

    }

    public static void Reset() {
        playerProgress = 0;
        playerHealth = GameLoopGameController_Script.maxPlayerHealth;
    }

    public static void SaveToFile() {

    }
}
