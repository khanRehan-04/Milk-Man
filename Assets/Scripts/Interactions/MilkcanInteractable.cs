using UnityEngine;
using UnityEngine.UI;

public class MilkcanInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform holdPoint;  // Player's hand position
    [SerializeField] private Button placeButton;   // Assign in Inspector

    public bool isHeld = false;  // Tracks whether the milkcan is held by the player
    private bool nearTruck = false;  // Tracks if the player is near a truck
    private Transform truckTrunkPoint;  // Position inside the truck
    private HoldableObject holdable;

    private void Start()
    {
        holdable = GetComponent<HoldableObject>();
        placeButton.gameObject.SetActive(false);  // Hide the place button initially
        placeButton.onClick.AddListener(PlaceInTruck);  // Add listener for button click
    }

    private void OnDestroy()
    {
        placeButton.onClick.RemoveListener(PlaceInTruck);  // Cleanup
    }

    public void Interact()
    {
        if (!MilkcanManager.Instance.CanPickNewCan()) return;

        if (!isHeld)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        AudioManager.Instance.PlaySFX("action");
        if (!MilkcanManager.Instance.CanPickNewCan()) return;

        isHeld = true; // Set flag for UI logic
        MilkcanManager.Instance.HoldCan(this);

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        holdable?.PickUp(); // 👈 Changes layer to hand

        FindObjectOfType<TruckTrunk>()?.UnregisterCan(this);
    }

    private void PlaceInTruck()
    {
        AudioManager.Instance.PlaySFX("action");
        if (truckTrunkPoint == null || !MilkcanManager.Instance.IsHolding(this)) return;

        isHeld = false;
        MilkcanManager.Instance.ReleaseHeldCan();

        transform.SetParent(truckTrunkPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        holdable?.Drop(); // 👈 Changes layer to world

        placeButton.gameObject.SetActive(false);
        FindObjectOfType<TruckTrunk>()?.RegisterPlacedCan(this);
    }


    // Called from TruckTrunk script to set proximity and show the button
    public void SetTruckProximity(bool isNear, Transform trunkPoint = null)
    {
        nearTruck = isNear;
        truckTrunkPoint = trunkPoint;

        // Show place button only if currently held and near truck
        placeButton.gameObject.SetActive(isHeld && isNear);
    }

    public void Release()
    {
        isHeld = false;
        MilkcanManager.Instance.ReleaseHeldCan();
    }
}
