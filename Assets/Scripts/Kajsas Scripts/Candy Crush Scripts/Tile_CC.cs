using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer rend; 

    public Vector2Int Position; // Positionen för brickan på rutnätet

    void Start()
    {
        rend = GetComponent<SpriteRenderer>(); // Hämta referens till SpriteRenderer-komponenten vid start
    }

    public void Select()
    {
        rend.color = Color.grey; // Ändra färgen på brickan till grå när den väljs
    }

    public void Unselect()
    {
        rend.color = Color.white; // Återställ färgen på brickan till vit när den avväljs
    }


    //Denna metod kan man ta bort sen, men måste kunna testa spelet på datorn. 
    private void OnMouseDown()
    {
        if (Input.touchCount == 0) // Kontrollera om det inte finns någon pekning på skärmen (endast för musinteraktion)
        {
            OnTilePressed(); // Anropa metod för att hantera när en bricka trycks ned
        }
    }

    void Update()
    {
        foreach (Touch touch in Input.touches) // Iterera genom alla touch-interaktioner
        {
            if (touch.phase == TouchPhase.Began) // Kontrollera om en touch-interaktion började
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); // Konvertera pekpositionen till världskoordinater
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero); // Gör en raycast för att avgöra träff

                if (hit.collider != null && hit.collider.gameObject == gameObject) // Kontrollera träff
                {
                    OnTilePressed(); // Hantera när en bricka trycks ned
                    break; // Avbryt loopen eftersom vi har hanterat en touch-interaktion
                }
            }
        }
    }

    private void OnTilePressed()
    {
        if (selected != null) // Kontrollera om det redan finns en vald bricka
        {
            if (selected == this) // Om den valda brickan är samma som den klickade brickan
                return; // Avbryt funktionen

            selected.Unselect(); // Avvälj den tidigare valda brickan
            if (Vector2Int.Distance(selected.Position, Position) == 1) // Kontrollera om den klickade brickan är granne med den valda brickan
            {
                GridManager.Instance.SwapTiles(Position, selected.Position); // Byt plats på brickorna
                selected = null; // Återställ referensen till den valda brickan
            }
            else
            {
                selected = this; 
                Select();
            }
        }
        else
        {
            selected = this;
            Select(); 
        }
    }
}
