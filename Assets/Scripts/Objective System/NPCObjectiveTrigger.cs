using UnityEngine;

public class NPCObjectiveTrigger : MonoBehaviour
{
    public UIManager uiManager;
    public ObjectiveManager objectiveManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var objective = objectiveManager.objectives[objectiveManager.currentObjectiveIndex];
            uiManager.ShowObjectivePanel(objective);

            AcceptObjective();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideObjectivePanel();
        }
    }

    public void AcceptObjective()
    {
        //uiManager.HideObjectivePanel();
        objectiveManager.StartObjective();
    }
}
