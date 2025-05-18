using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool isCityCompleted = false; // Tracks city completion status
    private string nextScene = "MainMenu";

    protected override void Awake()
    {
        base.Awake();

        // Load the saved state
        isCityCompleted = PlayerPrefs.GetInt("CityCompleted", 0) == 1;
    }

    // Mark city as completed and save it to PlayerPrefs
    public void CompleteCity()
    {
        isCityCompleted = true;
        PlayerPrefs.SetInt("CityCompleted", 1);
        PlayerPrefs.Save();
    }

    // Load the correct gameplay scene
    public void LoadGameplay()
    {
        nextScene = isCityCompleted ? "VillageScene" : "CityScene";
        SceneManager.LoadScene("SceneLoader");
    }

    // Load Main Menu
    public void LoadMainMenu()
    {
        nextScene = "MainMenu";
        SceneManager.LoadScene("SceneLoader");
    }

    // Reset game progress
    public void ResetGame()
    {
        isCityCompleted = false;
        PlayerPrefs.DeleteKey("CityCompleted");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Game reset. All PlayerPrefs cleared.");
    }

    // Get the next scene to load
    public string GetNextScene()
    {
        return nextScene;
    }
}
