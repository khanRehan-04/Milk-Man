using UnityEngine;

public class MiniMapTargetManager : MonoBehaviour
{
    [SerializeField] private VehicleManager vehicleManager; // Reference to VehicleManager
    [SerializeField] private bl_MiniMap miniMap; // Reference to MiniMap script
    [SerializeField] private GameObject player; // Reference to FirstPersonController GameObject
    [SerializeField] private RCC_CarControllerV3 carController; // Reference to Car controller

    void Start()
    {
        // Validate references
        if (vehicleManager == null)
        {
            Debug.LogError("VehicleManager reference not assigned in MiniMapTargetSwitcher.", gameObject);
            enabled = false;
            return;
        }
        if (miniMap == null)
        {
            Debug.LogError("MiniMap reference not assigned in MiniMapTargetSwitcher.", gameObject);
            enabled = false;
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player reference not assigned in MiniMapTargetSwitcher.", gameObject);
            enabled = false;
            return;
        }
        if (carController == null)
        {
            Debug.LogError("CarController reference not assigned in MiniMapTargetSwitcher.", gameObject);
            enabled = false;
            return;
        }

        // Subscribe to VehicleManager events
        vehicleManager.OnEnterCar += OnEnterCarHandler;
        vehicleManager.OnExitCar += OnExitCarHandler;

        miniMap.SetTarget(player);
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (vehicleManager != null)
        {
            vehicleManager.OnEnterCar -= OnEnterCarHandler;
            vehicleManager.OnExitCar -= OnExitCarHandler;
        }
    }

    private void OnEnterCarHandler()
    {
        // Set the minimap target to the car
        miniMap.SetTarget(carController.gameObject);
        Debug.Log("MiniMap target switched to Car.");
    }

    private void OnExitCarHandler()
    {
        // Set the minimap target to the player
        miniMap.SetTarget(player);
        Debug.Log("MiniMap target switched to Player.");
    }
}