using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBrickTower : MonoBehaviour
{
    [SerializeField] private GameObject brickTowerPrefab;
    [SerializeField] private GameObject canvas;

    public void InstantiateBrickTower()
    {
        Instantiate(brickTowerPrefab, canvas.transform);
    }
}
