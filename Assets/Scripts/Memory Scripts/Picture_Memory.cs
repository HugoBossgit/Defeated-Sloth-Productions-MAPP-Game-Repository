using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture_Memory : MonoBehaviour
{

    private Material firstMaterial;
    private Material secondMaterial;

    private Quaternion currentRotation;

    [HideInInspector]
    public bool revealed = false;

    private PictureManager_Memory pictureManager;
    private bool clicked = false;
    private bool isRotating = false;
    private int index;

    public void SetIndex(int id)
    {
        index = id;
    }

    public int GetIndex()
    {
        return index;
    }

    void Start()
    {
        revealed = false;
        clicked = false;
        pictureManager = GameObject.Find("PictureManager").GetComponent<PictureManager_Memory>();
        currentRotation = gameObject.transform.rotation;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !isRotating) 
            {
                if (!clicked && !revealed)
                {
                    pictureManager.currentPuzzleState = PictureManager_Memory.PuzzleState.PuzzleRotating;
                    StartCoroutine(LoopRotation(45, false));
                    clicked = true;
                    isRotating = true; 
                }
            }
        }
    }


    private void OnMouseDown()
    {
        if(clicked == false)
        {
            pictureManager.currentPuzzleState = PictureManager_Memory.PuzzleState.PuzzleRotating;
            StartCoroutine(LoopRotation(45, false));
            clicked = true;
        }
    }



    public void FlipBack()
    {
        if (gameObject.activeSelf)
        {
            pictureManager.currentPuzzleState = PictureManager_Memory.PuzzleState.PuzzleRotating;
            revealed = false;
            StartCoroutine(LoopRotation(45, true));
        }
    }

    IEnumerator LoopRotation(float angle, bool firstMat)
    {
        var rot = 0f;
        const float direction = 1f;
        const float rotSpeed = 180f;
        const float rotSpeed1 = 90.0f;
        var startAngle = angle;
        var assigned = false;

        if (firstMat)
        {
            while(rot < angle)
            {
                var step = Time.deltaTime * rotSpeed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * direction);
                if(rot >= (startAngle - 2) && assigned == false)
                {
                    ApplyFirstMaterial();
                    assigned = true;
                }

                rot += (1 * step * direction);
                yield return null;
            }
        }
        else
        {
            while(angle > 0)
            {
                float step = Time.deltaTime * rotSpeed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * direction);
                angle -= (1 * step * direction);
                yield return null;
            }
        }

        gameObject.GetComponent<Transform>().rotation = currentRotation;

        if (!firstMat)
        {
            revealed = true;
            ApplySecondMaterial();
            pictureManager.CheckPicture();
        }
        else
        {
            pictureManager.puzzleRevealedNumber = PictureManager_Memory.RevealedState.NoRevealed;
            pictureManager.currentPuzzleState = PictureManager_Memory.PuzzleState.CanRotate;
        }

        clicked = false;
    }

    public void SetFirstMaterial(Material mat, string texturePath)
    {
        firstMaterial = mat;
        firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void SetSecondMaterial(Material mat, string texturePath)
    {
        secondMaterial = mat;
        secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = firstMaterial;
    }

    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = secondMaterial;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void ResetIsRotating()
    {
        isRotating = false;
    }
}
