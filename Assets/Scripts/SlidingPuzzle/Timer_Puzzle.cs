using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_Puzzle : MonoBehaviour
{
    float countdown = 20;

    public Text tex;

    // Update is called once per frame
    void Update()
     {
        StartCoroutine(delay(0.5f));
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        double seconds = System.Math.Round(countdown, 0);
        tex.text = seconds.ToString();
     }

    private IEnumerator delay(float duration)
    {
        yield return new WaitForSeconds(duration);
        
    }
}
