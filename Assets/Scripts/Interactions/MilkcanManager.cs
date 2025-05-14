using UnityEngine;

public class MilkcanManager : MonoBehaviour
{
    public static MilkcanManager Instance { get; private set; }

    public MilkcanInteractable CurrentlyHeldCan { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanPickNewCan()
    {
        return CurrentlyHeldCan == null;
    }

    public void HoldCan(MilkcanInteractable can)
    {
        // If there's already a can held, release it first before holding the new one
        if (CurrentlyHeldCan != null)
        {
            CurrentlyHeldCan.Release();  // Release the currently held can before holding a new one
        }

        CurrentlyHeldCan = can;
        can.isHeld = true;  // Mark this can as held
    }

    public void ReleaseHeldCan()
    {
        if (CurrentlyHeldCan != null)
        {
            CurrentlyHeldCan.isHeld = false;  // Release the can
        }
        CurrentlyHeldCan = null;
    }

    public bool IsHolding(MilkcanInteractable can)
    {
        return CurrentlyHeldCan == can;
    }
}
