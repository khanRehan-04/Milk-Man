using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject pauseMenuUI;

    public void PauseGame()
    {
        //AdmobManager.instance?.ShowInterstitialAd();

        AudioManager.Instance?.PlaySFX("click");
        // Activate the pause menu UI
        pauseMenuUI.SetActive(true);

        // Stop the game
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        AudioManager.Instance?.PlaySFX("click");
        // Deactivate the pause menu UI
        pauseMenuUI.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        AudioManager.Instance?.PlaySFX("click");
        // Resume time before restarting
        Time.timeScale = 1f;

        // Call GameManager to load the gameplay scene
        GameManager.Instance?.LoadGameplay();
    }

    public void LoadHome()
    {
        AudioManager.Instance?.PlaySFX("click");
        // Resume time before returning to the main menu
        Time.timeScale = 1f;

        // Call GameManager to load the main menu
        GameManager.Instance?.LoadMainMenu();
    }
}
