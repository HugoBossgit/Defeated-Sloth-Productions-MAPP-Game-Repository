using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMoving : MonoBehaviour
{
    public RectTransform panel;
    public float minHeight = 400f;
    public float maxHeight = 600f;
    public float speed = 1f;

    private bool isExpanding = true;

    void Update()
    {
        float newHeight = panel.sizeDelta.y + (isExpanding ? speed : -speed) * Time.deltaTime * 100;

        if (newHeight >= maxHeight)
        {
            newHeight = maxHeight;
            isExpanding = false;
        }
        else if (newHeight <= minHeight)
        {
            newHeight = minHeight;
            isExpanding = true;
        }

        panel.sizeDelta = new Vector2(panel.sizeDelta.x, newHeight);
    }
}
