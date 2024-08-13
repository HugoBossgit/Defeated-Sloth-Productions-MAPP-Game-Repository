using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopBackgroundBehaivourScript : MonoBehaviour
{
    void Start()
    {
        //Sätt skärmens höjd till trippla kamerastorleken, eftersom att bilden här är mycket mindre så det gör jag för att täcka hela skärmen
        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Camera.main.aspect;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //Hämta spritens storlek
        float spriteHeight = spriteRenderer.bounds.size.y;
        float spriteWidth = spriteRenderer.bounds.size.x;

        //Räkna ut dess skala på denna storlek
        float heightScale = screenHeight / spriteHeight;
        float widthScale = screenWidth / spriteWidth;

        //Använd den större skalan för att se till att hela skärmen täcks
        float scale = Mathf.Max(heightScale, widthScale);
        transform.localScale = new Vector3(scale, scale, 1);

        //Placera spriten i mitten
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }
}
