using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture_Memory : MonoBehaviour
{

    private Material firstMaterial;
    private Material secondMaterial;

    private Quaternion currentRotation;

    void Start()
    {
        currentRotation = gameObject.transform.rotation;
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        StartCoroutine(LoopRotation(45, false));
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
            ApplySecondMaterial();
        }
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
}
