using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{
    public DeliveryObjective[] objectives;
    public GameObject indicatorParticlePrefab;
    public UIManager uiManager;
    public GameObject finalObjectiveObject;
    public GameObject secondLastObjectiveObject;

    [Header("Scene References")]
    public GameObject popup; // Drag your Popup GameObject here in Inspector

    public int currentObjectiveIndex = 0;
    private int currentDeliveryIndex = 0;
    public bool isTimerRunning = false;
    public float remainingTime;
    private GameObject activeIndicator;

    private void Start()
    {
        LoadObjectiveIndex();
        if (uiManager != null)
        {
            uiManager.Initialize(this); // Initialize UIManager
        }
    }

    public void StartObjective()
    {
        if (currentObjectiveIndex >= objectives.Length)
        {
            Debug.Log("All objectives completed. Clearing ALL saved progress.");
            PlayerPrefs.DeleteAll();
            if (uiManager != null)
            {
                uiManager.ShowCompletionScreen();
            }
            return;
        }

        if (currentObjectiveIndex == objectives.Length - 2 && secondLastObjectiveObject != null)
        {
            secondLastObjectiveObject.SetActive(true);
        }

        if (currentObjectiveIndex == objectives.Length - 1 && finalObjectiveObject != null)
        {
            finalObjectiveObject.SetActive(true);
        }

        remainingTime = objectives[currentObjectiveIndex].timeLimit;
        isTimerRunning = true;
        ActivateNextDelivery();
        if (uiManager != null)
        {
            uiManager.ShowObjectivePanel(objectives[currentObjectiveIndex]);
            uiManager.UpdateTimerUI(); // Initial UI update
        }
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (isTimerRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (uiManager != null)
            {
                uiManager.UpdateTimerUI(); // Update UI each frame
            }
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

        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
        }

        activeIndicator = Instantiate(indicatorParticlePrefab, targetPosition, Quaternion.identity);
        activeIndicator.SetActive(true);    

        DeliveryZone deliveryZone = activeIndicator.GetComponent<DeliveryZone>();
        if (deliveryZone != null && popup != null)
        {
            deliveryZone.SetPopup(popup);
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
                uiManager.ShowCompletionScreen();
            }
            PlayerPrefs.DeleteAll();
            uiManager.GoToHome();
            return;
        }

        currentObjectiveIndex++;
        SaveObjectiveIndex();
        if (uiManager != null)
        {
            uiManager.ShowCompletionScreen();
        }
    }

    private void FailObjective()
    {
        isTimerRunning = false;
        if (uiManager != null)
        {
            uiManager.ShowFailureScreen();
            uiManager.UpdateTimerUI(); // Ensure UI reflects timer stop
        }
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
        PlayerPrefs.DeleteAll();
        currentObjectiveIndex = 0;
        currentDeliveryIndex = 0;
        isTimerRunning = false;
        remainingTime = 0f;
        if (uiManager != null)
        {
            uiManager.UpdateTimerUI(); // Update UI to reflect reset
        }
        foreach (var objective in objectives)
        {
            objective.ResetObjective();
        }
    }
}