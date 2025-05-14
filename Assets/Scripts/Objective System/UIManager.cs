using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject completionPanel;
    public GameObject failurePanel;
    public GameObject objectivePanel;
    public TextMeshProUGUI objectiveText;
    private ObjectiveManager objectiveManager;

    public void Initialize(ObjectiveManager manager)
    {
        objectiveManager = manager;
    }

    public void ShowObjectivePanel(DeliveryObjective objective)
    {
        AudioManager.Instance.PlaySFX("popup");
        objectiveText.text = objective.objectiveDescription;
        objectivePanel.SetActive(true);
    }

    public void HideObjectivePanel()
    {
        objectivePanel.SetActive(false);
    }

    public void ShowCompletionScreen()
    {
        AudioManager.Instance.PlaySFX("complete");
        completionPanel.SetActive(true);
    }

    public void ShowFailureScreen()
    {
        AudioManager.Instance.PlaySFX("fail");
        failurePanel.SetActive(true);
    }

    public void ReloadScene()
    {
        GameManager.Instance.LoadGameplay();
    }

    public void GoToHome()
    {
        GameManager.Instance.LoadMainMenu();
    }

}
