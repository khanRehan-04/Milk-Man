using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    private float splashDuration = 8f; // Total splash screen duration (7 seconds)
    public Image loadingBar; // Reference to the loading bar Image

    private void Start()
    {
        StartCoroutine(EnsureGameManagerInitialized());
    }

    // Ensure GameManager is initialized before starting the scene loading
    private IEnumerator EnsureGameManagerInitialized()
    {
        while (GameManager.Instance == null)
        {
            yield return null; // Wait for the GameManager to be initialized
        }

        // Once GameManager is initialized, proceed with splash screen logic
        StartCoroutine(ShowSplashAndLoadScene());
    }

    private IEnumerator ShowSplashAndLoadScene()
    {
        string sceneToLoad = GameManager.Instance.GetNextScene();
        Debug.Log($"Loading Scene: {sceneToLoad}");

        // Start async loading early but prevent scene activation
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;

        float elapsedTime = 0f;

        // Fill the bar gradually over the entire splash duration
        while (elapsedTime < splashDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the fill amount based on how far along the splash time is
            float splashProgress = Mathf.Clamp01(elapsedTime / splashDuration);

            // Map the first 85% of splash progress normally and the last 15% to async loading progress
            if (elapsedTime <= splashDuration * 0.9f)
            {
                // Initial 85% is just time-based fill (0% to 85%)
                loadingBar.fillAmount = splashProgress * 0.9f;
            }
            else
            {
                // Last 15% is mapped to async loading (85% to 100%)
                float asyncProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                loadingBar.fillAmount = 0.9f + asyncProgress * 0.1f;

                // If scene is loaded enough, allow activation
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
