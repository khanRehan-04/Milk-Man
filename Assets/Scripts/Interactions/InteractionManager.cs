using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactRadius = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Button handButton;
    [SerializeField] private Camera playerCamera;

    private IInteractable[] currentInteractables;
    private Outline currentOutline; // Stores the currently outlined object

    private void Start()
    {
        if (handButton != null)
        {
            handButton.gameObject.SetActive(false);
            handButton.onClick.AddListener(OnHandButtonPressed);
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned! Please assign it in the Inspector.");
        }
    }

    private void Update()
    {
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactRadius, interactableLayer))
        {
            IInteractable[] interactables = hit.collider.GetComponents<IInteractable>();
            if (interactables.Length > 0)
            {
                currentInteractables = interactables;
                handButton.gameObject.SetActive(true); // Show hand button

                // Enable Outline if found on the object
                Outline outline = hit.collider.GetComponent<Outline>();
                if (outline != null)
                {
                    if (currentOutline != outline) // Avoid redundant enabling
                    {
                        DisableCurrentOutline(); // Disable previous outline
                        currentOutline = outline;
                        currentOutline.enabled = true; // Enable new outline
                    }
                }
                return;
            }
        }

        // No interactable detected, reset state
        DisableCurrentOutline();
        currentInteractables = null;
        handButton.gameObject.SetActive(false);
    }

    private void DisableCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }

    private void OnHandButtonPressed()
    {
        if (currentInteractables != null)
        {
            foreach (IInteractable interactable in currentInteractables)
            {
                interactable.Interact();
            }

            // Explicitly clear the interactables and reset the UI
            DisableCurrentOutline();
            currentInteractables = null;
            handButton.gameObject.SetActive(false);
        }
    }
}
