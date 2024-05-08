using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using TMPro;

public class MoveUIObject_DD : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform correctForm;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI timerText;


    [SerializeField] private AudioClip dropSound;
    [SerializeField] private float timeRemaining = 15f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 offset;
    private bool locked;
    private AudioSource audioSource;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

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

                audioSource.pitch = Random.Range(0.8f, 1.5f);
                audioSource.PlayOneShot(dropSound);

                if (eventTrigger != null)
                {
                    eventTrigger.enabled = false; // Inaktivera EventTrigger
                }

                //Om alla element är låsta har man vunnit
                if (GameObject.FindObjectsOfType<MoveUIObject_DD>().All(obj => obj.locked))
                {
                    Data.playerWin = true;
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

    private void Update()
    {
        // Uppdatera och visa timer
        if (!locked)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                Data.playerLose = true;
                losePanel.SetActive(true);
            }
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        // Uppdatera UI-texten för att visa den återstående tiden
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = seconds.ToString();
    }
}
