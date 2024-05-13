using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float maxTime = 5f;
   
    private bool doCountDown;
    private float timeRemaining;
    private TextMeshProUGUI countdownText;

    void Start() {
        countdownText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() {
        timeRemaining = maxTime;
        DisableCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (doCountDown) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                UpdateUI();
            }
            else {
                timeRemaining = 0;
            }
        }
    }

    void UpdateUI() {
        int timeInSeconds = (int)Math.Floor(timeRemaining % 60);
        if (timeInSeconds < 0) {
            timeInSeconds = 0;
        }
        countdownText.text = timeInSeconds.ToString();
    }

    public void EnableCountDown() {
        doCountDown = true;
    }

    public void DisableCountDown() {
        doCountDown = false;
    }
}
