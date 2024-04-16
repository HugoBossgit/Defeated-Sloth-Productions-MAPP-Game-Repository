using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Brick
{
    private int durability;
    //private SpriteIndex spriteIndex;
    private Color color;

    public int Durability { get { return durability; } set { durability = value; } }
    public Color Color { get { return color; } }

    public Brick(int durability, Color color)
    {
        this.durability = durability;
        this.color = color;
    }
}
public class BrickTower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panelPrefab;
    //[SerializeField] private Sprite[] sprites;
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject gameCompleteText;

    private List<Brick> bricks;
    private Stack<GameObject> panels;
    private int countdown = 5;

    // Start is called before the first frame update
    void Start()
    {
        bricks = new List<Brick>();
        panels = new Stack<GameObject>();
        StartGame();
        AddBricks();
        GeneratePanels();
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

            Color color = colors[durability];

            bricks.Add(new Brick(durability, color));
        }
    }

    void GeneratePanels()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            GameObject panel = Instantiate(panelPrefab, this.transform);
            panel.GetComponent<Image>().color = bricks[i].Color;
            RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
            panelRectTransform.anchoredPosition = new Vector2(0, 100 + 95 * i);
            panels.Push(panel);
        }
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
        }
        else
        {
            panels.Peek().GetComponent<Image>().color = colors[durability];
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
    }

    void CountDown()
    {
        countdown -= 1;
    }

    void GameComplete()
    {
        gameOverPanel.SetActive(true);
        gameCompleteText.SetActive(true);
    }
}
