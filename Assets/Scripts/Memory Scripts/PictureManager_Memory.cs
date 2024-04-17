using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureManager_Memory : MonoBehaviour
{

    public Picture_Memory picturePrefab;
    public Transform picSpawnPos;
    public Vector2 startPosition = new Vector2(-2.15f, 3.62f);

    [Space]
    [Header("End Game Screen")]
    public GameObject endGamePanel;
    public GameObject yourScoreText;
    public GameObject endTimeText;

    public enum GameState
    {
        NoAction,
        MovingOnPositions,
        DeletingPuzzles,
        FlipBack,
        Checking,
        GameEnd
    }

    public enum PuzzleState
    {
        PuzzleRotating,
        CanRotate
    }

    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    }

    [HideInInspector]
    public GameState currentGameState;
    [HideInInspector]
    public PuzzleState currentPuzzleState;
    [HideInInspector]
    public RevealedState puzzleRevealedNumber;

    [HideInInspector]
    public List<Picture_Memory> pictureList;

    private Vector2 _offset = new Vector2(1.5f, 1.52f);

    private List<Material> materialList = new List<Material>();
    private List<string> texturePathList = new List<string>();

    private Material firstMaterial;
    private string firstTexturePath;

    private int firstRevealedPic;
    private int secondRevealedPic;
    private int revealedPicNum = 0;
    private int picToDestroy1;
    private int picToDestroy2;

    private int pairNumbers;
    private int removedPairs;

    private Timer_Memory gameTimer;

    void Start()
    {

        currentGameState = GameState.NoAction;
        currentPuzzleState = PuzzleState.CanRotate;
        puzzleRevealedNumber = RevealedState.NoRevealed;
        revealedPicNum = 0;
        firstRevealedPic = -1;
        secondRevealedPic = -1;

        removedPairs = 0;
        pairNumbers = (int)GameSettings_Memory.Instance.GetPairNumber();

        gameTimer = GameObject.Find("Main Camera").GetComponent<Timer_Memory>();

        LoadMaterials();

        if(GameSettings_Memory.Instance.GetPairNumber() == GameSettings_Memory.EpairNumber.E10Pairs)
        {
            currentGameState = GameState.MovingOnPositions;
            SpawnPictureMesh(4, 5, startPosition, _offset, false);
            MovePicture(4, 5, startPosition, _offset);
        }

    }

    private void Update()
    {
        if (currentGameState == GameState.DeletingPuzzles)
        {
            if(currentPuzzleState == PuzzleState.CanRotate)
            {
                DestoryPicture();
                CheckGameEnd();
            }
        }

        if(currentGameState == GameState.FlipBack)
        {
            if(currentPuzzleState == PuzzleState.CanRotate)
            {
                FlipBack();
            }
        }

        if(currentGameState == GameState.GameEnd)
        {
            if (pictureList[firstRevealedPic].gameObject.activeSelf == false &&
                pictureList[secondRevealedPic].gameObject.activeSelf == false &&
                endGamePanel.activeSelf == false)
            {
                ShowEndGameInformation();
            }
        }
    }

    private void ShowEndGameInformation()
    {
        endGamePanel.SetActive(true);
        yourScoreText.SetActive(true);

        var timer = gameTimer.GetCurrentTime();
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minutes.ToString("00" + ":" + seconds.ToString("00"));
        endTimeText.GetComponent<Text>().text = newText;
    }

    private bool CheckGameEnd()
    {
        if(removedPairs == pairNumbers && currentGameState != GameState.GameEnd)
        {
            currentGameState = GameState.GameEnd;
            gameTimer.StopTimer();
        }
        return (currentGameState == GameState.GameEnd);
    }

    public void CheckPicture()
    {
        currentGameState = GameState.Checking;
        revealedPicNum = 0;

        for(int id = 0; id < pictureList.Count; id++)
        {
            if (pictureList[id].revealed && revealedPicNum < 2)
            {
                if (revealedPicNum == 0)
                {
                    firstRevealedPic = id;
                    revealedPicNum++;
                }
                else if (revealedPicNum == 1)
                {
                    secondRevealedPic = id;
                    revealedPicNum++;
                }  
            }
        }

        if(revealedPicNum == 2)
        {
            if (pictureList[firstRevealedPic].GetIndex() == pictureList[secondRevealedPic].GetIndex() && firstRevealedPic != secondRevealedPic)
            {
                currentGameState = GameState.DeletingPuzzles;
                picToDestroy1 = firstRevealedPic;
                picToDestroy2 = secondRevealedPic;
            }
            else
            {
                currentGameState = GameState.FlipBack;
            }
        }

        currentPuzzleState = PictureManager_Memory.PuzzleState.CanRotate;

        if(currentGameState == GameState.Checking)
        {
            currentGameState = GameState.NoAction;
        }
    }

    private void DestoryPicture()
    {
        puzzleRevealedNumber = RevealedState.NoRevealed;
        pictureList[picToDestroy1].Deactivate();
        pictureList[picToDestroy2].Deactivate();
        revealedPicNum = 0;
        removedPairs++;
        currentGameState = GameState.NoAction;
        currentPuzzleState = PuzzleState.CanRotate;
    }

    private void FlipBack()
    {
        pictureList[firstRevealedPic].FlipBack();
        pictureList[secondRevealedPic].FlipBack();

        pictureList[firstRevealedPic].revealed = false;
        pictureList[secondRevealedPic].revealed = false;

        puzzleRevealedNumber = RevealedState.NoRevealed;
        currentGameState = GameState.NoAction;

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
            o.SetIndex(rndMatIndex);
            o.revealed = false;
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