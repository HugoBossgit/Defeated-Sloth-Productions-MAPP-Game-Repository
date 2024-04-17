using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMiniGame : MonoBehaviour
{

    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    float fishPosition;
    float fishDestination;

    float fishTimer;

    [SerializeField] float timerMultiplicator;

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    private void Update()
    {
        fishTimer -= Time.deltaTime;
        if(fishTimer < 0f)
        {
            fishTimer = Random.value * timerMultiplicator;

            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
}
