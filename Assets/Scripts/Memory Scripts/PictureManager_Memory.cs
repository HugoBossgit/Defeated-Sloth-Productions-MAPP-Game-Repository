using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager_Memory : MonoBehaviour
{

    public Picture_Memory picturePrefab;
    public Transform picSpawnPos;
    public Vector2 startPosition = new Vector2(-2.15f, 3.62f);

    [HideInInspector]
    public List<Picture_Memory> pictureList;

    private Vector2 _offset = new Vector2(1.5f, 1.52f);

    private List<Material> materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();

    private Material firstMaterial;
    private string firstTexturePath;
    
    void Start()
    {
        LoadMaterials();
        SpawnPictureMesh(4, 5, startPosition, _offset, false);
        MovePicture(4, 5, startPosition, _offset);
    }

    private void LoadMaterials()
    {

    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < columns; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture_Memory)Instantiate(picturePrefab, picSpawnPos.position, picSpawnPos.transform.rotation);

                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                pictureList.Add(tempPicture);
            }
        }
    }

    private void MovePicture(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for(var col = 0; col < columns; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, pictureList[index]));
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Picture_Memory obj)
    {
        var randomDis = 7;
        while(obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return 0;
        }
    }
}