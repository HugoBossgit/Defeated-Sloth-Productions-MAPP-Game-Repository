using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Script : MonoBehaviour
{
    [SerializeField] private GameObject workInProgressPanel, creditsPanel, settingsPanel, fadeBackgroundPanel, newGameButton, resumeGameButton, returnToPlayButton;

    [SerializeField] private List<Button> playCreditsSettingsButtons;


    private void Start()
    {
        workInProgressPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        fadeBackgroundPanel.SetActive(false);
        DeactivateNewResumeReturnButtons();
    }



    private void DisableAllButtons()
    {
        foreach (Button button in playCreditsSettingsButtons)
        {
            button.interactable = false;
        }
    }



    private void EnableAllButtons()
    {
        foreach (Button button in playCreditsSettingsButtons)
        {
            button.interactable = true;
        }
    }



    public void OpenWorkInProgressPanel()
    {
        DisableAllButtons();
        workInProgressPanel.SetActive(true);
        fadeBackgroundPanel.SetActive(true);
    }



    public void CloseWorkInProgressPanel()
    {
        EnableAllButtons();
        workInProgressPanel.SetActive(false);
        fadeBackgroundPanel.SetActive(false);
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
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        Data.walking = false;
        Data.running = false;
        Data.Reset();
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
