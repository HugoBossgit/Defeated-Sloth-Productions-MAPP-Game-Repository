using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayPanelScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuControllerGameObject;
    private Animator animator;
    private MainMenu_Script script;

    private void Start()
    {
        animator = GetComponent<Animator>();
        script = mainMenuControllerGameObject.GetComponent<MainMenu_Script>();
    }
    public void OnAnimationExitFinish()
    {
        script.EnableAllButtons();
        script.fadeBackgroundPanel.SetActive(false);
        script.howToPlayPanel.SetActive(false);
    }
}
