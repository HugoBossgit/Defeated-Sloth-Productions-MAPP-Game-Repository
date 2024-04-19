using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MoveUIObject_DD : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform correctForm;
    public GameObject winPanel;


    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 offset;
    private bool locked;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //OnPointerDown för att interagera med UI element
    public void OnPointerDown(PointerEventData eventData)
    {
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    //Triggas när spelaren rör ett UI element
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!locked)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!locked)
        {
            float distance = Vector2.Distance(rectTransform.anchoredPosition, correctForm.anchoredPosition);

            if (distance <= 50f) // Anpassa avståndet efter behov
            {
                rectTransform.anchoredPosition = correctForm.anchoredPosition;
                locked = true; // Lås UI-elementet om det hamnar på rätt plats
                EventTrigger eventTrigger = GetComponent<EventTrigger>();
                if (eventTrigger != null)
                {
                    eventTrigger.enabled = false; // Inaktivera EventTrigger
                }

                //Om alla element är låsta har man vunnit
                if (GameObject.FindObjectsOfType<MoveUIObject_DD>().All(obj => obj.locked))
                {
                    winPanel.SetActive(true);
                }
            }
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!locked)
        {
            rectTransform.anchoredPosition = eventData.position + offset;
        }
    }
}
