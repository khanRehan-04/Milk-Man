using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;          // The main menu panel
    public GameObject ratePanel;         // The rate panel
    public GameObject policyPanel;       // The privacy panel
    public GameObject exitPanel;         // The exit panel
    public GameObject settingsPanel;     // The settings panel

    public GameObject panelsLayoutScreen; // Parent for rate, policy, settings, and exit panels

    private void Start()
    {
        GoBackToMainMenu();
    }

    public void OnPlay()
    {
        OnButtonClickSFX();
        GameManager.Instance?.LoadGameplay();
    }
  
    // Method to show the Rate Panel
    public void ShowRatePanel()
    {
        ShowOptionsPanel(ratePanel);
    }

    // Method to show the Privacy Panel
    public void ShowPolicyPanel()
    {
        ShowOptionsPanel(policyPanel);
    }

    // Method to show the Exit Panel
    public void ShowExitPanel()
    {
        ShowOptionsPanel(exitPanel);
    }

    // Method to show the Settings Panel
    public void ShowSettingsPanel()
    {
        ShowOptionsPanel(settingsPanel);
    }

    // Method to go back to the Main Menu
    public void GoBackToMainMenu()
    {
        OnButtonClickSFX();        //CLick SFX
        mainMenu.SetActive(true);  // Show the main menu

        // Deactivate all other panels and the options parent panel
        HideAllPanels();
    }

    // Method to hide all panels (including parent panel)
    private void HideAllPanels()
    {
        ratePanel.SetActive(false);
        policyPanel.SetActive(false);
        exitPanel.SetActive(false);
        settingsPanel.SetActive(false);

        panelsLayoutScreen.SetActive(false); // Hide the parent container as well
    }

    // Method to show a specific panel within the options parent
    private void ShowOptionsPanel(GameObject panelToShow)
    {
        OnButtonClickSFX();        //CLick SFX
        HideAllPanels();                  // Hide all other panels
        mainMenu.SetActive(false);        // Hide the main menu
        panelsLayoutScreen.SetActive(true); // Show the parent container
        panelToShow.SetActive(true);      // Show the selected panel inside the parent
    }

    public void ExitGame()
    {
        OnButtonClickSFX();
        Application.Quit();
    }

    // Method to handle the "Yes" button click on the rate panel
    public void OnRateYesButtonClick()
    {
        OnButtonClickSFX();
        //UnityEngine.iOS.Device.RequestStoreReview();
    }

    // Method to handle the "Yes" button click on the privacy panel
    public void OnPolicyYesButtonClick()
    {
        OnButtonClickSFX();
        //Application.OpenURL("https://www.freeprivacypolicy.com/live/dc22c2a4-3a7a-4baa-ac99-971ada99b2d6");
    }

    private void OnButtonClickSFX()
    {
        AudioManager.Instance?.PlaySFX("click");
    }
}
