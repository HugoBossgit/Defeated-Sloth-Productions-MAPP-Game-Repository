using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public GameObject GameOverMenu;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI scoreText;

    public int GridDimension = 8;
    public int startingMoves = 20;
    public float Distance = 1.2f;

    private GameObject[,] Grid;
    private int _numMoves;

    public int NumMoves
    {
        get
        {
            return _numMoves;
        }

        set
        {
            _numMoves = value;
            movesText.text = _numMoves.ToString();
        }
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }

    //Singleton pattern för att instansiera från Tile
    public static GridManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        Score = 0;
        NumMoves = startingMoves;
        GameOverMenu.SetActive(false);
    }

    private void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }
    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3((GridDimension - 1) * Distance / 2.0f, (GridDimension - 1) * Distance / 2.0f, 0);

        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                List<Sprite> possibleSprites = new List<Sprite>(Sprites); // 1

                // Välj vilken sprite som ska användas för den här cellen
                Sprite left1 = GetSpriteAt(column - 1, row); // 2
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2) // 3
                {
                    possibleSprites.Remove(left1); // 4
                }

                Sprite down1 = GetSpriteAt(column, row - 1); // 5
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }

                GameObject newTile = Instantiate(TilePrefab);
                newTile.transform.parent = transform;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;

                // Tilldela slumpmässig sprite från de möjliga spritesen
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];

                // Lägg till Tile-komponenten
                Tile tile = newTile.AddComponent<Tile>();
                tile.Position = new Vector2Int(column, row);
                newTile.transform.parent = transform;

                Grid[column, row] = newTile;
            }
        }
    }


    Sprite GetSpriteAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
            || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    SpriteRenderer GetSpriteRendererAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
             || row < 0 || row >= GridDimension)
            return null;
        GameObject tile = Grid[column, row];
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        //returnerar renderern istället för spriten i getSpriteAt
        return renderer;
    }

    bool CheckMatches()
    {
        //Hashset tillåter inte duplikater och söker igenom element snabbt
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();
        for (int row = 0; row < GridDimension; row++)
        {
            //Söker igenom grid från högra botten rad för rad uppåt
            for (int column = 0; column < GridDimension; column++)
            {
                //tar renderer för nuvarande cell
                SpriteRenderer current = GetSpriteRendererAt(column, row);

                //returnerar lista av renderers som är involverade i en match horizontalt
                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(column, row, current.sprite);
                if (horizontalMatches.Count >= 2)
                {
                    //om det är 2 eller fler så har vi match
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }
                //samma som innan fast vertikalt
                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(column, row, current.sprite);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }
        //sätter alla renderers i en match till null och eliminerar dem
        foreach (SpriteRenderer renderer in matchedTiles)
        {
            renderer.sprite = null;
        }
        Score += matchedTiles.Count;
        return matchedTiles.Count > 0;
    }



    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    void FillHoles()
    {
        //går igenom grid
        for (int column = 0; column < GridDimension; column++)
        {
            for (int row = 0; row < GridDimension; row++)
            {
                //om nuvarande cell är tom,flyttar vi de översta cellerna, cellerna flyttas
                //ner tills nuvarande cell inte längre är emtpy
                while (GetSpriteRendererAt(column, row).sprite == null)
                {
                    //ny cykel börjar som flyttar ner cellerna en position
                    for (int filler = row; filler < GridDimension - 1; filler++)
                    {
                        //för att flytta så assignar vi den lägre cellen till värdet av den övre cellen
                        SpriteRenderer current = GetSpriteRendererAt(column, filler);
                        SpriteRenderer next = GetSpriteRendererAt(column, filler + 1);
                        current.sprite = next.sprite;
                    }
                    //Till slut så selectar vi en random sprite och assignar till cellen
                    SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1);
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)];
                }
            }
        }
    }

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        GameObject tile1 = Grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        GameObject tile2 = Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();

        // Byter plats på brickorna
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;

        // Minska antalet drag med 1
        NumMoves--;

        // Uppdatera antalet drag i UI
        movesText.text = NumMoves.ToString();

        if (!CheckMatches())
        {
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;

            ShuffleTiles();
        }
        else
        {
            do
            {
                FillHoles();
            } while (CheckMatches());
            if (NumMoves <= 0)
            {
                NumMoves = 0; GameOver();
            }
        }
    }

    void ShuffleTiles()
    {
        // Skapa en lista för att hålla de aktuella brickorna
        List<SpriteRenderer> allTiles = new List<SpriteRenderer>();

        // Lägg till alla brickor i listan
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                allTiles.Add(GetSpriteRendererAt(column, row));
            }
        }

        // Slumpa om ordningen på brickorna
        for (int i = 0; i < allTiles.Count; i++)
        {
            int randomIndex = Random.Range(i, allTiles.Count);
            SpriteRenderer temp = allTiles[i];
            allTiles[i] = allTiles[randomIndex];
            allTiles[randomIndex] = temp;
        }

        // Uppdatera brickornas placering i griden med den nya ordningen
        int index = 0;
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                SpriteRenderer currentTile = GetSpriteRendererAt(column, row);
                currentTile.sprite = allTiles[index].sprite;
                index++;
            }
        }
    }



    void GameOver()
    {
        PlayerPrefs.SetInt("score", Score);
        GameOverMenu.SetActive(true);
    }
}