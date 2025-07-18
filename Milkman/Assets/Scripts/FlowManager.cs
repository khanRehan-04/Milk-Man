using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    MeetMilkman,
    LoadMilk,
    DriveTruck // Final state
}

public class FlowManager : MonoBehaviour
{
    public static FlowManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.None;
    private bool flowEnabled = true;

    [SerializeField] private UIManager uiManager;

    public UnityEvent<GameState> OnStateChanged = new UnityEvent<GameState>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        AdvanceState();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign the reference to the new UIManager in the reloaded scene
        uiManager = FindObjectOfType<UIManager>();

        if (uiManager == null)
            Debug.LogWarning("UIManager not found in scene!");

        ResetFlow(); // Start the flow again
    }


    private void AdvanceState(bool showPopup = true)
    {
        switch (CurrentState)
        {
            case GameState.None:
                CurrentState = GameState.MeetMilkman;
                if (showPopup) ShowPopup("Meet the milkman!");
                break;

            case GameState.MeetMilkman:
                CurrentState = GameState.LoadMilk;
                if (showPopup) ShowPopup("Load the milk onto the truck!");
                break;

            case GameState.LoadMilk:
                CurrentState = GameState.DriveTruck;
                if (showPopup) ShowPopup("Get in the truck and start driving!");
                break;

            case GameState.DriveTruck:
                Debug.Log("Final state reached: DriveTruck");
                return;
        }

        OnStateChanged.Invoke(CurrentState);
    }

    public void CompleteAction(bool showPopup = true)
    {
        if (flowEnabled && CurrentState != GameState.DriveTruck)
        {
            AdvanceState(showPopup);
        }
        else
        {
            Debug.Log("No further state transitions allowed.");
        }
    }

    public void ShowCurrentStatePopup()
    {
        switch (CurrentState)
        {
            case GameState.None:
                ShowPopup("Game starting soon...");
                break;

            case GameState.MeetMilkman:
                ShowPopup("Meet the milkman!");
                break;

            case GameState.LoadMilk:
                ShowPopup("Load the milk onto the truck!");
                break;

            case GameState.DriveTruck:
                ShowPopup("Get in the truck and start driving!");
                break;
        }
    }

    private void ShowPopup(string message)
    {
        if (uiManager != null && uiManager.popup != null)
        {
            var textComponent = uiManager.popup.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
                textComponent.text = message;

            uiManager.popup.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UIManager or popup not assigned in FlowManager!");
        }
    }

    public bool CanPerformAction(GameState actionState)
    {
        return flowEnabled && actionState == CurrentState;
    }

    public void ResetFlow()
    {
        Debug.Log("Flow reset.");
        flowEnabled = true;
        CurrentState = GameState.None;
        AdvanceState();
    }

    public void DisableFlow()
    {
        flowEnabled = false;
    }
}
