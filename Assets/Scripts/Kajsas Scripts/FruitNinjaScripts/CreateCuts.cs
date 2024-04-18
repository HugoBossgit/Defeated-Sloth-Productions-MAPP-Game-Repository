using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCuts : MonoBehaviour
{
    [SerializeField] private GameObject cut;
    [SerializeField] private float cutDestroyTime;

    private bool dragging = false;

    private Vector2 swipeStart;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            swipeStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            createCut();
        }
    }
    private void createCut()
    {
        dragging = false;
        Vector2 swipeEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject cut = Instantiate(this.cut, swipeStart, Quaternion.identity);
        cut.GetComponent<LineRenderer>().SetPosition(0, swipeStart);
        cut.GetComponent<LineRenderer>().SetPosition(1, swipeEnd);
        Vector2[] colliderPoints = new Vector2[2];
        colliderPoints[0] = new Vector2(0.0f, 0.0f);
        colliderPoints[1] = swipeEnd - swipeStart;
        cut.GetComponent<EdgeCollider2D>().points = colliderPoints;
        Destroy(cut.gameObject, cutDestroyTime);
    }
}