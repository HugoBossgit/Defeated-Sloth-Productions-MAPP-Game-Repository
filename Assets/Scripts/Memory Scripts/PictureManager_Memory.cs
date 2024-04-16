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
    private Vector2 _offsetFor15Pairs = new Vector2(1.08f, 1.22f);
    private Vector2 _offsetFor20Pairs = new Vector2(1.08f, 1.0f);
    private Vector3 newScaleDown = new Vector3(0.9f, 0.9f, 0.001f);

    private List<Material> materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();

    private Material firstMaterial;
    private string firstTexturePath;
    
    void Start()
    {
        LoadMaterials();

        if(GameSettings_Memory.Instance.GetPairNumber() == GameSettings_Memory.EpairNumber.E10Pairs)
        {
            SpawnPictureMesh(4, 5, startPosition, _offset, false);
            MovePicture(4, 5, startPosition, _offset);
        }
        else if (GameSettings_Memory.Instance.GetPairNumber() == GameSettings_Memory.EpairNumber.E15Pairs)
        {
            SpawnPictureMesh(5, 6, startPosition, _offsetFor15Pairs, false);
            MovePicture(5, 6, startPosition, _offsetFor15Pairs);
        }
        else if (GameSettings_Memory.Instance.GetPairNumber() == GameSettings_Memory.EpairNumber.E20Pairs)
        {
            SpawnPictureMesh(5, 8, startPosition, _offsetFor20Pairs, true);
            MovePicture(5, 8, startPosition, _offsetFor20Pairs);
        }

    }

    private void LoadMaterials()
    {
        var materialFilePath = GameSettings_Memory.Instance.GetMaterialDirectoryName();
        var textureFilePath = GameSettings_Memory.Instance.GetPuzzleCategoryTextureDirectory();
        var pairNumber = (int)GameSettings_Memory.Instance.GetPairNumber();
        const string matBaseName = "Pic";
        var firstMaterialName = "Back";

        for(var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material;
            materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            texturePathList.Add(currentTextureFilePath);
        }

        firstTexturePath = textureFilePath + firstMaterialName;
        firstMaterial = Resources.Load(materialFilePath + firstMaterialName, typeof(Material)) as Material;
    }

    private void SpawnPictureMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < columns; col++)
        {
            for(int row = 0; row < rows; row++)
            {
                var tempPicture = (Picture_Memory)Instantiate(picturePrefab, picSpawnPos.position, picturePrefab.transform.rotation);

                if (scaleDown)
                {
                    tempPicture.transform.localScale = newScaleDown;
                }

                tempPicture.name = tempPicture.name + 'c' + col + 'r' + row;
                pictureList.Add(tempPicture);
            }
        }

        ApplyTextures();
    }

    public void ApplyTextures()
    {
        var rndMatIndex = Random.Range(0, materialList.Count);
        var AppliedTimes = new int[materialList.Count];

        for (int i = 0; i < materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }

        foreach(var o in pictureList)
        {
            var randPrevious = rndMatIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[rndMatIndex] >= 2 || ((randPrevious == rndMatIndex) && !forceMat))
            {
                rndMatIndex = Random.Range(0, materialList.Count);
                counter++;
                if(counter > 100)
                {
                    for(var j = 0; j < materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            rndMatIndex = j;
                            forceMat = true;
                        }
                    }

                    if(forceMat == false)
                    {
                        return;
                    }
                }
            }

            o.SetFirstMaterial(firstMaterial, firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(materialList[rndMatIndex], texturePathList[rndMatIndex]);
            AppliedTimes[rndMatIndex] += 1;
            forceMat = false;
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