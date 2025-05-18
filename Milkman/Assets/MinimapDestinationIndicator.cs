using UnityEngine;

public class MinimapDestinationIndicator : MonoBehaviour
{
    [Header("Scene References")]
    public Transform player;
    public Transform destination;

    [Header("UI References")]
    public RectTransform indicator;

    [Header("Settings")]
    public float hideDistance = 3f;

    void Update()
    {
        if (player == null || destination == null || indicator == null)
            return;

        // Direction from player to destination (XZ plane)
        Vector3 toDestination = destination.position - player.position;
        Vector2 dir2D = new Vector2(toDestination.x, toDestination.z);

        // Hide if close
        if (dir2D.magnitude < hideDistance)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        indicator.gameObject.SetActive(true);

        // Calculate angle from up to the direction
        float angle = Vector2.SignedAngle(Vector2.up, dir2D);

        // Flip it 180 degrees (if it's pointing the wrong way)
        angle += 180f;

        // Apply the rotation to the arrow
        indicator.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
