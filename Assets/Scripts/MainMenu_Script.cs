using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Script : MonoBehaviour
{
    [SerializeField] private GameObject workInProgressPanel, creditsPanel, settingsPanel;

    [SerializeField] private List<Button> classCreditsSettingsButtons;



    private void Start()
    {
        workInProgressPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }



    private void disableAllButtons()
    {
        foreach (Button button in classCreditsSettingsButtons)
        {
            button.enabled = false;
        }
    }



    private void enableAllButtons()
    {
        foreach (Button button in classCreditsSettingsButtons)
        {
            button.enabled = true;
        }
    }



    public void openWorkInProgressPanel()
    {
        disableAllButtons();
        workInProgressPanel.SetActive (true);
    }



    public void closeWorkInProgressPanel()
    {
        enableAllButtons();
        workInProgressPanel.SetActive(false);
    }



    public void openCreditsPanel()
    {
        disableAllButtons();
        creditsPanel.SetActive (true);
    }



    public void closeCreditsPanel()
    {
        enableAllButtons();
        creditsPanel.SetActive (false);
    }



    public void openSettingsPanel()
    {
        disableAllButtons();
        settingsPanel.SetActive (true);
    }



    public void closeSettingsPanel()
    {
        enableAllButtons();
        settingsPanel.SetActive (false);
    }



    public void playAsWarrior()
    {
        
    }


    public void playAsHunter()
    {
        openWorkInProgressPanel();
    }



    public void playAsMage()
    {
        openWorkInProgressPanel();
    }
}
