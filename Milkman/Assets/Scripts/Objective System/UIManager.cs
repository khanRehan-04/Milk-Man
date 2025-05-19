using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject completionPanel;
    public GameObject failurePanel;
    public GameObject objectivePanel;
    public GameObject Timer;
    public GameObject popup; // Reference to popup GameObject
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI timerText; // Timer text for MM:SS
    public Image timerBarFill; // Timer bar fill image
    private ObjectiveManager objectiveManager;
    private bool hasShownObjectivePanel = false; // Tracks if ShowObjectivePanel was called
    private float popupCycleDelay = 8.5f; // Delay between popup cycles (adjustable)

    private void Start()
    {
        if (popup != null)
        {
            popup.SetActive(true); // Activate popup at start
            StartCoroutine(HidePopupAfterDelay(3f)); // Hide after 3 seconds
            StartCoroutine(PopupCycle()); // Start popup cycle
        }
    }

    public void Initialize(ObjectiveManager manager)
    {
        objectiveManager = manager;
    }

    public void ShowObjectivePanel(DeliveryObjective objective)
    {
        AudioManager.Instance.PlaySFX("popup");
        objectiveText.text = objective.objectiveDescription;
        objectivePanel.SetActive(true);
        Timer.SetActive(true);
        hasShownObjectivePanel = true; // Stop popup cycle
        UpdateTimerUI(); // Update timer UI when objective starts
    }

    public void HideObjectivePanel()
    {
        objectivePanel.SetActive(false);
    }

    public void ShowCompletionScreen()
    {
        AudioManager.Instance.PlaySFX("complete");
        completionPanel.SetActive(true);
        objectivePanel.SetActive(false); // Hide timer UI on completion
        Timer.SetActive(false);
    }

    public void ShowFailureScreen()
    {
        AudioManager.Instance.PlaySFX("fail");
        failurePanel.SetActive(true);
        objectivePanel.SetActive(false); // Hide timer UI on failure
        Timer.SetActive(false);
    }

    public void ReloadScene()
    {
        GameManager.Instance.LoadGameplay();
    }

    public void GoToHome()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void UpdateTimerUI()
    {
        if (objectiveManager == null || !objectiveManager.isTimerRunning)
        {
            if (timerText != null) timerText.text = "00:00";
            if (timerBarFill != null) timerBarFill.fillAmount = 0f;
            return;
        }

        float remainingTime = objectiveManager.remainingTime;
        float timeLimit = objectiveManager.objectives[objectiveManager.currentObjectiveIndex].timeLimit;

        // Update timer text (MM:SS)
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Update timer bar fill
        if (timerBarFill != null && timeLimit > 0)
        {
            timerBarFill.fillAmount = remainingTime / timeLimit;
        }
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popup != null)
        {
            popup.SetActive(false);
        }
    }

    private IEnumerator PopupCycle()
    {
        while (!hasShownObjectivePanel)
        {
            yield return new WaitForSeconds(popupCycleDelay); // Wait for cycle delay
            if (popup != null && !hasShownObjectivePanel)
            {
                popup.SetActive(true); // Show popup
                yield return new WaitForSeconds(3f); // Keep visible for 3 seconds
                if (popup != null && !hasShownObjectivePanel)
                {
                    popup.SetActive(false); // Hide popup
                }
            }
        }
    }
}