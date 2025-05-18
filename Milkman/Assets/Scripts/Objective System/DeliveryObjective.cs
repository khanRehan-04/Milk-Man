using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "MilkDelivery/Objective")]
public class DeliveryObjective : ScriptableObject
{
    public string objectiveName;
    [TextArea] public string objectiveDescription;
    public Vector3[] deliveryLocations; // Store positions instead of Transforms
    public float timeLimit;

    public bool IsCompleted
    {
        get => PlayerPrefs.GetInt(objectiveName, 0) == 1;
        set => PlayerPrefs.SetInt(objectiveName, value ? 1 : 0);
    }

    public void ResetObjective()
    {
        PlayerPrefs.SetInt(objectiveName, 0);
    }
}
