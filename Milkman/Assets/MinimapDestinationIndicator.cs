using UnityEngine;

public class MinimapDestinationIndicator : MonoBehaviour
{
    public Transform player; // Reference to player indicator in world
    public Transform destination; // Reference to destination indicator in world
    public RectTransform pointerUI; // Reference to UI pointer (this object)
    public RectTransform minimapRect; // RectTransform of the minimap for orientation reference
    public float smoothSpeed = 5f; // Smoothing speed
    public float minAngleChange = 1f; // Minimum angle change to update

    private Quaternion targetRotation;
    private Quaternion currentRotation;
    private float lastAngle;

    void Start()
    {
        currentRotation = pointerUI.localRotation;
        targetRotation = currentRotation;
        lastAngle = 0f;

        // Debug the minimap's rotation
        Debug.Log("Minimap Rect Rotation: " + minimapRect.localRotation.eulerAngles);
    }

    void Update()
    {
        if (player == null || destination == null || pointerUI == null)
            return;

        // Calculate direction from player to destination
        Vector3 direction = destination.position - player.position;
        direction.y = 0f; // Flatten to XZ plane

        // Avoid issues with zero direction
        if (direction.magnitude < 0.01f)
        {
            return;
        }

        // Normalize the direction
        direction.Normalize();

        // Calculate the angle of the direction relative to world +Z
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Adjust for player's rotation (facing -X, 270 degrees)
        Vector3 playerForward = player.forward;
        playerForward.y = 0f;
        playerForward.Normalize();
        float playerAngle = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;

        // Relative angle from player's forward to destination
        float relativeAngle = directionAngle - playerAngle;

        // Adjust to align minimap "up" (+Z) with UI "up" (0 degrees)
        relativeAngle -= 90f; // Corrects the 180-degree error seen in the image

        // Normalize the angle to [0, 360) range
        relativeAngle = (relativeAngle + 360f) % 360f;

        // Only update if the angle changes significantly
        if (Mathf.Abs(relativeAngle - lastAngle) > minAngleChange)
        {
            targetRotation = Quaternion.Euler(0f, 0f, relativeAngle);
            lastAngle = relativeAngle;
        }

        // Smooth the rotation
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, smoothSpeed * Time.deltaTime);
        pointerUI.localRotation = currentRotation;
    }
}