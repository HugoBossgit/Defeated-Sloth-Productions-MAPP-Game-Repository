using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds;
    [SerializeField] private GameObject self;
    void Start()
    {
        Invoke("DestroySelf", destroyAfterSeconds);
    }

    void DestroySelf() {
        Destroy(self);
    }
}
