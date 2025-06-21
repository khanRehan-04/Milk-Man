using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    None,
    MeetMilkman,
    LoadMilk,
    DriveTruck,
    Completed
}

public class FlowManager : MonoBehaviour
{
    public static FlowManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.None;

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

        AdvanceState();
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
                if (showPopup) ShowPopup("Get in the truck and start the delivery!");
                break;
            case GameState.DriveTruck:
                CurrentState = GameState.Completed;
                if (showPopup) ShowPopup("Delivery completed!");
                break;
        }

        OnStateChanged.Invoke(CurrentState);
    }

    public void CompleteAction(bool showPopup = true)
    {
        if (CurrentState != GameState.Completed)
        {
            AdvanceState(showPopup);
        }
    }

    private void ShowPopup(string message)
    {
        if (uiManager != null && uiManager.popup != null)
        {
            uiManager.popup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
            uiManager.popup.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UIManager or popup not assigned in FlowManager!");
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
                ShowPopup("Get in the truck and start delivery!");
                break;
            case GameState.Completed:
                ShowPopup("Delivery completed!");
                break;
        }
    }
    public bool CanPerformAction(GameState actionState)
    {
        return actionState == CurrentState;
    }
}