using UnityEngine;
using DG.Tweening; // Required for DOTween
using UnityEngine.UI; // Required for Button

public class VehicleManager : MonoBehaviour
{
    public GameObject player;
    public GameObject carCamera;
    public GameObject carCanvas;
    public RCC_CarControllerV3 carController;

    // The child objects that should remain active and be re-parented to the car
    public GameObject childObject1; // This one will match car rotation with an offset
    public GameObject childObject2; // This one will just follow the car

    // The default rotation offset for childObject1
    private Vector3 defaultRotationOffset = new Vector3(90f, 270f, 0f);

    // Distance and radius for sphere cast for exit checks
    public float exitCheckDistance = 3f;
    public float sphereCastRadius = 0.5f; // Radius of the sphere for the cast
    public Vector3 castStartOffset = new Vector3(0f, 1f, 0f); // Offset to avoid car's own collider

    // For detecting nearby Point objects
    private float pointCheckDistance = 25f; // Distance to check for Point-tagged objects
    public Button carExitButton; // Reference to the exit button
    private DOTweenAnimation exitButtonAnimation; // Reference to the DOTween animation
    private bool isInCar = false; // Tracks if player is in the car

    public void EnterCar()
    {
        // Deactivate the entire player, but leave the child objects active
        player.SetActive(false);

        // Keep the child objects active
        if (childObject1 != null) childObject1.SetActive(true);
        if (childObject2 != null) childObject2.SetActive(true);

        // Parent the child objects to the car
        if (childObject1 != null)
        {
            childObject1.transform.SetParent(carController.transform);

            // Match rotation to car but account for the offset
            childObject1.transform.rotation = carController.transform.rotation * Quaternion.Euler(defaultRotationOffset);
        }

        if (childObject2 != null)
        {
            childObject2.transform.SetParent(carController.transform);
        }

        // Enable the car camera and canvas
        carCamera.SetActive(true);
        carCanvas.SetActive(true);

        // Enable car controls
        carController.enabled = true;

        // Get reference to the exit button and its DOTween animation
        if (carCanvas != null)
        {
            //carExitButton = GetComponentInChildren<Button>();
            if (carExitButton != null)
            {
                exitButtonAnimation = carExitButton.GetComponent<DOTweenAnimation>();
                if (exitButtonAnimation == null)
                {
                    Debug.LogWarning("No DOTweenAnimation found on the car exit button!");
                }
            }
            else
            {
                Debug.LogWarning("No Button found in carCanvas!");
            }
        }

        // Set flag to start checking for Point objects
        isInCar = true;
    }

    public void ExitCar()
    {
        // Stop checking for Point objects
        isInCar = false;

        // Starting position for sphere casts (car position with offset to avoid car's collider)
        Vector3 startPos = carController.transform.position + castStartOffset;

        // Check which side is clear for exit using sphere casts
        Vector3 rightDirection = carController.transform.right;
        Vector3 leftDirection = -carController.transform.right;

        bool isRightClear = IsExitClear(startPos, rightDirection);
        bool isLeftClear = IsExitClear(startPos, leftDirection);

        // Choose exit position: prefer right, then left, or default to right if both are blocked
        Vector3 exitPosition = carController.transform.position + rightDirection * exitCheckDistance;
        if (!isRightClear && isLeftClear)
        {
            exitPosition = carController.transform.position + leftDirection * exitCheckDistance;
        }
        else if (!isRightClear && !isLeftClear)
        {
            Debug.LogWarning("Both sides are blocked! Defaulting to right side.");
            exitPosition = carController.transform.position + rightDirection * exitCheckDistance; // Fallback to right side
        }

        // Reactivate the player GameObject and set its position
        player.SetActive(true);
        player.transform.position = exitPosition;

        // Parent the child objects back to the player
        if (childObject1 != null)
        {
            childObject1.transform.SetParent(player.transform);
            // Reset rotation to player, accounting for the offset
            childObject1.transform.rotation = player.transform.rotation * Quaternion.Euler(defaultRotationOffset);
        }

        if (childObject2 != null)
        {
            childObject2.transform.SetParent(player.transform);
        }

        // Disable the car camera and canvas
        carCamera.SetActive(false);
        carCanvas.SetActive(false);

        // Disable car controls
        carController.enabled = false;
    }

    private void Update()
    {
        // Only check for Point objects if in the car
        if (isInCar)
        {
            CheckForPointObjects();
        }
    }

    private void CheckForPointObjects()
    {
        // Find all GameObjects with the "Point" tag
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag("Point");
        bool pointFound = false;

        // Check distance to each Point object
        foreach (GameObject point in pointObjects)
        {
            float distance = Vector3.Distance(carController.transform.position, point.transform.position);
            if (distance <= pointCheckDistance)
            {
                pointFound = true;
                break;
            }
        }

        // Play or pause the animation based on proximity to Point
        if (pointFound && exitButtonAnimation != null)
        {
            exitButtonAnimation.DOPlay(); // Play the DOTween animation
        }
        else if (!pointFound && exitButtonAnimation != null)
        {
            exitButtonAnimation.DOPause(); // Pause the animation if no Point is nearby
        }
    }

    private bool IsExitClear(Vector3 startPos, Vector3 direction)
    {
        // Cast a sphere from startPos in the given direction
        RaycastHit hit;
        bool isClear = !Physics.SphereCast(startPos, sphereCastRadius, direction, out hit, exitCheckDistance);
        return isClear; // Return true if no colliders are hit
    }

    // Optional: Visualize the sphere casts and point check radius in the Scene view for debugging
    private void OnDrawGizmos()
    {
        if (carController != null)
        {
            Vector3 startPos = carController.transform.position + castStartOffset;
            Vector3 rightDirection = carController.transform.right;
            Vector3 leftDirection = -carController.transform.right;

            // Draw right sphere cast path
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPos, sphereCastRadius); // Starting sphere
            Gizmos.DrawLine(startPos, startPos + rightDirection * exitCheckDistance); // Path
            Gizmos.DrawWireSphere(startPos + rightDirection * exitCheckDistance, sphereCastRadius); // End sphere

            // Draw left sphere cast path
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(startPos, sphereCastRadius); // Starting sphere
            Gizmos.DrawLine(startPos, startPos + leftDirection * exitCheckDistance); // Path
            Gizmos.DrawWireSphere(startPos + leftDirection * exitCheckDistance, sphereCastRadius); // End sphere

            // Draw point check radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(carController.transform.position, pointCheckDistance); // Point detection radius
        }
    }
}