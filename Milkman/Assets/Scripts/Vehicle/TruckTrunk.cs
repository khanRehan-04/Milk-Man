using UnityEngine;
using System.Collections.Generic;

public class TruckTrunk : MonoBehaviour
{
    [SerializeField] private Transform[] canSlots;  // Set in Inspector
    [SerializeField] private ObjectiveManager objectiveManager; // ✅ Reference set manually or via FindObjectOfType

    private Dictionary<int, MilkcanInteractable> slotMap = new Dictionary<int, MilkcanInteractable>();

    private void Awake()
    {
        for (int i = 0; i < canSlots.Length; i++)
        {
            slotMap[i] = null;  // Initialize slots to null
        }

        // ✅ Optional auto-assign if not set in Inspector
        if (objectiveManager == null)
        {
            objectiveManager = FindObjectOfType<ObjectiveManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var can = other.GetComponent<MilkcanInteractable>();
        if (can == null) return;

        int? slotIndex = GetSlotIndex(can);
        if (slotIndex.HasValue)
        {
            can.SetTruckProximity(true, canSlots[slotIndex.Value]);
        }
        else
        {
            int freeSlot = GetFirstFreeSlot();
            if (freeSlot != -1)
            {
                can.SetTruckProximity(true, canSlots[freeSlot]);
            }
            else
            {
                can.SetTruckProximity(true, null);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var can = other.GetComponent<MilkcanInteractable>();
        if (can != null)
        {
            can.SetTruckProximity(false, null);
        }
    }

    public void RegisterPlacedCan(MilkcanInteractable can)
    {
        if (IsAlreadyRegistered(can)) return;

        int index = GetFirstFreeSlot();
        if (index != -1)
        {
            slotMap[index] = can;
            TryAdvanceGameState(); // ✅ Check milk can count after placing
        }
    }

    public void UnregisterCan(MilkcanInteractable can)
    {
        foreach (var kvp in slotMap)
        {
            if (kvp.Value == can)
            {
                slotMap[kvp.Key] = null;
                break;
            }
        }
    }

    private int GetFirstFreeSlot()
    {
        foreach (var kvp in slotMap)
        {
            if (kvp.Value == null)
                return kvp.Key;
        }
        return -1;
    }

    private int? GetSlotIndex(MilkcanInteractable can)
    {
        foreach (var kvp in slotMap)
        {
            if (kvp.Value == can)
                return kvp.Key;
        }
        return null;
    }

    private bool IsAlreadyRegistered(MilkcanInteractable can)
    {
        return slotMap.ContainsValue(can);
    }

    private void TryAdvanceGameState()
    {
        var flow = FlowManager.Instance;

        if (flow != null && objectiveManager != null && flow.CurrentState == GameState.LoadMilk)
        {
            int requiredCans = objectiveManager.objectives[objectiveManager.currentObjectiveIndex].milkCanCount;
            int placedCans = GetPlacedCanCount();

            if (placedCans >= requiredCans)
            {
                flow.CompleteAction(); // ✅ Automatically move to DriveTruck
            }
        }
    }

    private int GetPlacedCanCount()
    {
        int count = 0;
        foreach (var slot in slotMap.Values)
        {
            if (slot != null) count++;
        }
        return count;
    }

    public void ResetSlots()
    {
        foreach (var key in new List<int>(slotMap.Keys))
        {
            slotMap[key] = null;
        }
    }
}
