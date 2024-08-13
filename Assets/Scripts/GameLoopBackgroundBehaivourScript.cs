using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopBackgroundBehaivourScript : MonoBehaviour
{
    void Start()
    {
        //S�tt sk�rmens h�jd till trippla kamerastorleken, eftersom att bilden h�r �r mycket mindre s� det g�r jag f�r att t�cka hela sk�rmen
        float screenHeight = Camera.main.orthographicSize * 3.0f;
        float screenWidth = screenHeight * Camera.main.aspect;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //H�mta spritens storlek
        float spriteHeight = spriteRenderer.bounds.size.y;
        float spriteWidth = spriteRenderer.bounds.size.x;

        //R�kna ut dess skala p� denna storlek
        float heightScale = screenHeight / spriteHeight;
        float widthScale = screenWidth / spriteWidth;

        //Anv�nd den st�rre skalan f�r att se till att hela sk�rmen t�cks
        float scale = Mathf.Max(heightScale, widthScale);
        transform.localScale = new Vector3(scale, scale, 1);

        //Placera spriten i mitten
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }
}