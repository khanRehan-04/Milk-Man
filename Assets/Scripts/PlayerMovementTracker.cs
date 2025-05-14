using UnityEngine;
using System.Collections;

public class PlayerMovementTracker : MonoBehaviour
{
    public GameObject popup;
    float idleTimeThreshold = 5f;

    private Vector3 lastPosition;
    private float idleTimer = 0f;
    private bool popupActive = false;
    private Coroutine popupRoutine;

    public static bool hasReachedEnd = false;

    void Start()
    {
        lastPosition = transform.position;
        popup.SetActive(false);
    }

    void Update()
    {
        if (hasReachedEnd) return;

        float movement = Vector3.Distance(transform.position, lastPosition);

        if (movement < 0.01f)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimeThreshold && !popupActive)
            {
                popup.SetActive(true);
                popupActive = true;

                if (popupRoutine != null)
                    StopCoroutine(popupRoutine);

                popupRoutine = StartCoroutine(HidePopupAfterDelay(3f));
            }
        }
        else
        {
            idleTimer = 0f;
        }

        lastPosition = transform.position;
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popup.SetActive(false);
        popupActive = false;
    }

    public void TriggerEndCutscene()
    {
        hasReachedEnd = true;

        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popup.SetActive(false);
        popupActive = false;

        Debug.Log("Cutscene Triggered");
        // Place your cutscene call here
    }
}
