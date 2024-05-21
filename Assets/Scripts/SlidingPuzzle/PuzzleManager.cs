using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PuzzleManager : MonoBehaviour
{
    public Text timerText;
    public GameObject startInfo;
    public GameObject winPanel;
    public GameObject losePanel;

    [SerializeField] private AudioClip dragSound;
    [SerializeField] private float timeRemaining = 20f;
    [SerializeField] private Transform board;
    [SerializeField] private Transform puzzleBit;


    private List<Transform> bitar;
    private int emptyLocation;
    private int size;
    private bool shuff = false;
    private AudioSource audioSource;


    private void CreatePuzzleBitar(float gapThic)
    {
        float width = 1 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform bit = Instantiate(puzzleBit, board);

                bitar.Add(bit);

                bit.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);

                bit.localScale = ((2 * width) - gapThic) * Vector3.one;
                bit.name = $"{(row * size) + col}";

                if ((row == size -1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    bit.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThic / 2;
                    Mesh mesh = bit.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));

                    mesh.uv = uv;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bitar = new List<Transform>();
        size = 3;
        startInfo.SetActive(true);
        StartGame();
        startInfo.SetActive(false);
        CreatePuzzleBitar(0.01f);
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                Data.playerLose = true;
            }
            UpdateTimerDisplay();

        }

        if (!shuff && CheckCom())
        {
            shuff = true;
            StartCoroutine(delay(0.5f));
        }

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(dragSound);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for(int i = 0; i < bitar.Count; i++)
                {
                    if (bitar[i] == hit.transform)
                    {
                        if (IfValidSwap(i, -size, size)) { break; }
                        if (IfValidSwap(i, +size, size)) { break; }
                        if (IfValidSwap(i, -1, 0)) { break; }
                        if (IfValidSwap(i, +1, size -1)) { break; }

                    }
                }
            }
        }
        
    }

    void StartGame()
    {
        startInfo.SetActive(true);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

    }

  

    private void UpdateTimerDisplay()
    {
        
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time Remaining: " + seconds.ToString();
    }

    private bool IfValidSwap(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            (bitar[i], bitar[i + offset]) = (bitar[i + offset], bitar[i]);
            (bitar[i].localPosition, bitar[i + offset].localPosition) = ((bitar[i + offset].localPosition, bitar[i].localPosition));
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCom()
    {
        for (int i = 0; i < bitar.Count; i++)
        {
            if (bitar[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator delay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuff = false;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size))
        {
            int rnd = Random.Range(0, size * size);

            if (rnd == last) { continue; }
            last = emptyLocation;

            if (IfValidSwap(rnd, -size, size))
            {
                count++;
            } else if (IfValidSwap(rnd, +size, size))
            {
                count++;
            } else if (IfValidSwap(rnd, -1, 0))
            {
                count++;
            } else if (IfValidSwap(rnd, +1, size -1))
            {
                count++;
            }
        }

    }

    public void WinOrLose()
    {
        if (CheckCom() && timeRemaining > 0)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }
    public void Win()
    {
        winPanel.SetActive(true);
        Data.playerWin = true;
        
        SceneManager.LoadScene(1);
    }

    public void Lose()
    {
        losePanel.SetActive(true);
        Data.playerLose = true;

        SceneManager.LoadScene(1);
        
    }





}
