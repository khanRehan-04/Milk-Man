using UnityEngine;
using System.Collections.Generic;

public class TruckTrunk : MonoBehaviour
{
    [SerializeField] private Transform[] canSlots;  // Set in Inspector
    private Dictionary<int, MilkcanInteractable> slotMap = new Dictionary<int, MilkcanInteractable>();

    private void Awake()
    {
        for (int i = 0; i < canSlots.Length; i++)
        {
            slotMap[i] = null;  // Initialize slots to null
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var can = other.GetComponent<MilkcanInteractable>();
        if (can == null) return;

        // Check if the milkcan is already in a slot
        int? slotIndex = GetSlotIndex(can);
        if (slotIndex.HasValue)
        {
            // Send back to the same slot if it's still in the truck
            can.SetTruckProximity(true, canSlots[slotIndex.Value]);
        }
        else
        {
            // Assign the can to a free slot if available
            int freeSlot = GetFirstFreeSlot();
            if (freeSlot != -1)
            {
                can.SetTruckProximity(true, canSlots[freeSlot]);
            }
            else
            {
                // No available slots
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
        // Prevent duplicate registration
        if (IsAlreadyRegistered(can)) return;

        int index = GetFirstFreeSlot();
        if (index != -1)
        {
            slotMap[index] = can;
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
}
