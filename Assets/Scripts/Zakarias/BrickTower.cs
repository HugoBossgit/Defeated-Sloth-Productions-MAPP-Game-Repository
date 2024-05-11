using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Brick
{
    private int durability;
    //private SpriteIndex spriteIndex;
    //private Color color;
    private Sprite sprite;

    public int Durability { get { return durability; } set { durability = value; } }
    //public Color Color { get { return color; } }
    public Sprite Sprite {get { return sprite; } }

    public Brick(int durability, Sprite sprite)
    {
        this.durability = durability;
        this.sprite = sprite;
    }

    /*public Brick(int durability, Color color)
    {
        this.durability = durability;
        this.color = color;
    }*/
}
public class BrickTower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private Color[] colors; //Not currently in use
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject gameCompleteText;
    [SerializeField] private int brickTop;
    [SerializeField] private int brickBottom;
    [SerializeField] private int brickSides;
    [SerializeField] private int brickSpacing;
    [SerializeField] private AudioClip tapSound;
    [SerializeField] private AudioClip breakSound;

    private List<Brick> bricks;
    private Stack<GameObject> panels;
    private Vector2 canvasSize;
    private int countdown = 5;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        bricks = new List<Brick>();
        panels = new Stack<GameObject>();
        StartGame();
        AddBricks();
        GeneratePanels();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (bricks.Count == 0)
        {
            CancelInvoke();
            GameComplete();
        }

        if (countdown < 1)
        {
            CancelInvoke();
            OutOfTime();
        }

    }

    void AddBricks()
    {
        for (int i = 0; i < 5; i++)
        {
            int durability = Random.Range(0, 5);

            bricks.Add(new Brick(durability, sprites[durability]));

            //Color color = colors[durability];

            //bricks.Add(new Brick(durability, color));
        }
    }

    void GeneratePanels()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            GameObject panel = Instantiate(panelPrefab, this.transform);
            panel.GetComponent<Image>().sprite = bricks[i].Sprite;
            RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
            adjustStretching(panelRectTransform, brickSides, brickTop - brickSpacing * i, brickSides, brickBottom + brickSpacing * i);
            panels.Push(panel);
        }
    }

    void adjustStretching(RectTransform panelRectTransform, float left, float top, float right, float bottom) {
        
        
        //Scaled based on Iphone 12 resolution 1170*2532
        
        canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        
        left = left/1170 * canvasSize.x;
        bottom = bottom/2532 * canvasSize.y;
        right = -(right/1170 * canvasSize.x);
        top = -(top/2532 * canvasSize.y);
        //Debug.Log(canvasSize);
        //Debug.Log(left + " " + top + " " + right + " " + bottom);
        panelRectTransform.offsetMin = new Vector2(left, bottom);
        panelRectTransform.offsetMax = new Vector2(right, top);
    }

    public void DamageTopBrick(int damage)
    {
        int brickCount = bricks.Count;
        if (brickCount == 0)
        {
            return;
        }
        int durability = bricks[brickCount - 1].Durability;
        durability -= damage;
        bricks[brickCount - 1].Durability = durability;
        UpdateTopPanel(durability);
    }

    void UpdateTopPanel(int durability)
    {
        if (durability < 0)
        {
            Destroy(panels.Peek());
            panels.Pop();
            bricks.RemoveAt(bricks.Count - 1);
            audioSource.PlayOneShot(breakSound, 0.7f);
        }
        else
        {
            panels.Peek().GetComponent<Image>().sprite = sprites[durability];
            audioSource.PlayOneShot(tapSound, 0.85f);
        }
    }

    void StartGame()
    {
        infoPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        outOfTimeText.SetActive(false);
        gameCompleteText.SetActive(false);
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);

        //Start Timer
        InvokeRepeating("CountDown", 1, 1);
    }

    void OutOfTime()
    {
        gameOverPanel.SetActive(true);
        outOfTimeText.SetActive(true);
        Data.playerLose = true;
        Debug.Log("Player lost brick game");
    }

    void CountDown()
    {
        countdown -= 1;
    }

    void GameComplete()
    {
        gameOverPanel.SetActive(true);
        gameCompleteText.SetActive(true);
        Data.playerWin = true;
        //Debug.Log("Player won brick game");
    }
}
