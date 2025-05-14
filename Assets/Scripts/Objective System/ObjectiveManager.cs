using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{
    public DeliveryObjective[] objectives;
    public GameObject indicatorParticlePrefab;
    public UIManager uiManager;
    public GameObject finalObjectiveObject;

    [Header("Scene References")]
    public GameObject popup; // Drag your Popup GameObject here in Inspector
    public MinimapDestinationIndicator minimapDestinationIndicator; // Reference to MinimapDestinationIndicator

    public int currentObjectiveIndex = 0;
    private int currentDeliveryIndex = 0;
    private bool isTimerRunning = false;
    private float remainingTime;
    private GameObject activeIndicator;

    private void Start()
    {
        LoadObjectiveIndex();
    }

    public void StartObjective()
    {
        if (currentObjectiveIndex >= objectives.Length)
        {
            Debug.Log("All objectives completed. Clearing ALL saved progress.");

            PlayerPrefs.DeleteAll(); // Completely clear all saved preferences

            if (uiManager != null)
            {
                uiManager.ShowCompletionScreen(); // Or use a custom method to show final game end
            }

            return;
        }

        if (currentObjectiveIndex == objectives.Length - 1 && finalObjectiveObject != null)
        {
            finalObjectiveObject.SetActive(true);
        }

        remainingTime = objectives[currentObjectiveIndex].timeLimit;
        isTimerRunning = true;
        ActivateNextDelivery();
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (isTimerRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        if (remainingTime <= 0)
        {
            FailObjective();
        }
    }

    private void ActivateNextDelivery()
    {
        if (currentDeliveryIndex >= objectives[currentObjectiveIndex].deliveryLocations.Length)
        {
            CompleteObjective();
            return;
        }

        Vector3 targetPosition = objectives[currentObjectiveIndex].deliveryLocations[currentDeliveryIndex];

        // Destroy the previous indicator if it exists
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
        }

        // Instantiate the new indicator at the target position
        activeIndicator = Instantiate(indicatorParticlePrefab, targetPosition, Quaternion.identity);

        // Assign popup to DeliveryZone if it exists
        DeliveryZone deliveryZone = activeIndicator.GetComponent<DeliveryZone>();
        if (deliveryZone != null && popup != null)
        {
            deliveryZone.SetPopup(popup);
        }

        // Set the destination in MinimapDestinationIndicator
        if (minimapDestinationIndicator != null)
        {
            minimapDestinationIndicator.destination = activeIndicator.transform; // Pass the indicator position
        }
    }

    public void DeliverMilk()
    {
        if (currentDeliveryIndex < objectives[currentObjectiveIndex].deliveryLocations.Length)
        {
            currentDeliveryIndex++;
            ActivateNextDelivery();
        }
    }

    private void CompleteObjective()
    {
        isTimerRunning = false;
        objectives[currentObjectiveIndex].IsCompleted = true;

        if (currentObjectiveIndex >= objectives.Length - 1)
        {
            if (uiManager != null)
            {
                uiManager.ShowCompletionScreen(); // Show final screen
            }
            PlayerPrefs.DeleteAll(); // Completely clear all saved preferences
            return;
        }

        currentObjectiveIndex++;
        SaveObjectiveIndex();
        uiManager.ShowCompletionScreen();
    }


    private void FailObjective()
    {
        isTimerRunning = false;
        uiManager.ShowFailureScreen();
    }

    private void SaveObjectiveIndex()
    {
        PlayerPrefs.SetInt("CurrentObjectiveIndex", currentObjectiveIndex);
    }

    private void LoadObjectiveIndex()
    {
        currentObjectiveIndex = PlayerPrefs.GetInt("CurrentObjectiveIndex", 0);
    }

    public void ResetAllObjectives()
    {
        PlayerPrefs.DeleteAll(); // Also clear everything when resetting
        currentObjectiveIndex = 0;
        foreach (var objective in objectives)
        {
            objective.ResetObjective();
        }
    }
}
