using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Script : MonoBehaviour
{
    [SerializeField] public GameObject creditsPanel, settingsPanel, fadeBackgroundPanel, newGameButton, resumeGameButton, returnToPlayButton, howToPlayPanel, transitionObject;

    [SerializeField] private List<Button> playCreditsSettingsButtons;

    private Animator howToPlayAnim, LoadSceneAnim;


    private void Start()
    {
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        fadeBackgroundPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        DeactivateNewResumeReturnButtons();
        howToPlayAnim = howToPlayPanel.GetComponent<Animator>();
        LoadSceneAnim = transitionObject.GetComponent<Animator>();
    }



    private void DisableAllButtons()
    {
        foreach (Button button in playCreditsSettingsButtons)
        {
            button.interactable = false;
        }
    }



    public void EnableAllButtons()
    {
        foreach (Button button in playCreditsSettingsButtons)
        {
            button.interactable = true;
        }
    }

    public void OpenCreditsPanel()
    {
        DisableAllButtons();
        creditsPanel.SetActive(true);
        fadeBackgroundPanel.SetActive(true);
    }

    public void CloseCreditsPanel()
    {
        EnableAllButtons();
        creditsPanel.SetActive(false);
        fadeBackgroundPanel.SetActive(false);
    }

    public void OpenHowToPlayPanel()
    {
        howToPlayPanel.SetActive(true);
        fadeBackgroundPanel.SetActive(true);
        DisableAllButtons();
        howToPlayAnim.SetBool("OnScreen", true);
        howToPlayAnim.SetBool("OffScreen", false);
    }

    public void CloseHowToPlayPanel()
    {
        howToPlayAnim.SetBool("OffScreen", true);
        howToPlayAnim.SetBool("OnScreen", false);
    }



    public void OpenSettingsPanel()
    {
        DisableAllButtons();
        settingsPanel.SetActive(true);
        fadeBackgroundPanel.SetActive(true);
    }



    public void CloseSettingsPanel()
    {
        EnableAllButtons();
        settingsPanel.SetActive(false);
        fadeBackgroundPanel.SetActive(false);
    }

    public void Play()
    {
        Data.returningToMainMenu = false;
        DisableAllButtons();
        fadeBackgroundPanel.SetActive(true);
        ActivatePlayResumeReturnButtons();

        if (Data.playerProgress > 0)
        {
            resumeGameButton.GetComponent<Button>().interactable = true;
            resumeGameButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            resumeGameButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            resumeGameButton.GetComponent<Button>().interactable = false;
            
        }
    }

    public void ResumeGame()
    {
        Data.walking = false;
        Data.running = false;
        StartCoroutine(LoadMainGameLoop());
    }

    public void NewGame()
    {
        Data.walking = false;
        Data.running = false;
        Data.Reset();
        StartCoroutine(LoadMainGameLoop());
    }

    private IEnumerator LoadMainGameLoop()
    {
        LoadSceneAnim.SetTrigger("Start");

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(1);
    }


    public void BackFromPlay()
    {
        EnableAllButtons();
        fadeBackgroundPanel.SetActive(false);
        DeactivateNewResumeReturnButtons();
    }

    private void ActivatePlayResumeReturnButtons()
    {
        newGameButton.SetActive(true);
        resumeGameButton.SetActive(true);
        returnToPlayButton.SetActive(true);
    }

    private void DeactivateNewResumeReturnButtons()
    {
        newGameButton.SetActive(false);
        resumeGameButton.SetActive(false);
        returnToPlayButton.SetActive(false);
    }
}
