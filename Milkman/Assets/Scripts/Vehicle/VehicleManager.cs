using UnityEngine;

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
    }

    public void ExitCar()
    {
        // Calculate the position to place the player when exiting
        Vector3 exitPosition = carController.transform.position + carController.transform.right *3f;

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
}
