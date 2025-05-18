using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // The car to follow
    public Vector3 offset = new Vector3(0, 5, -10); // Default camera offset
    public float smoothSpeed = 5f; // Smoothing speed

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        // Look at the car
        transform.LookAt(target);
    }
}
