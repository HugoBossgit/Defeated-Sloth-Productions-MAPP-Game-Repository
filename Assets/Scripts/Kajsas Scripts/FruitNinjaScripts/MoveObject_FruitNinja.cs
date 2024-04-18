using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject_FruitNinja : MonoBehaviour
{
    [SerializeField] float minXSpeed, maxXSpeed, minYSpeed, maxYSpeed;

    [SerializeField] float destroyTime;

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(minXSpeed, maxXSpeed),
            Random.Range(minYSpeed, maxYSpeed));
        Destroy(this.gameObject, this.destroyTime);
    }

    void Update()
    {
        
    }
}
