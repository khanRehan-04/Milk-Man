using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public Transform target; // Assign your target in Inspector
    public Vector3 rotationOffset = new Vector3(-80, 180, -90); // Your chosen values

    void Update()
    {
        if (target != null)
        {
            // Get direction toward the target
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Keep the arrow horizontal

            // If direction is valid
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Apply manual rotation offset
                targetRotation *= Quaternion.Euler(rotationOffset);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }
}
