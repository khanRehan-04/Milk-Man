using System.Collections;
using UnityEngine;

public class FigureAppearAndVanish : MonoBehaviour
{
    public GameObject figure;
    public Transform appearPosition;
    public float visibleDuration = 2f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || other.tag != "Player") return;

        triggered = true;
        StartCoroutine(AppearThenDisappear());
    }

    IEnumerator AppearThenDisappear()
    {
        figure.transform.position = appearPosition.position;
        figure.SetActive(true);

        yield return new WaitForSeconds(visibleDuration);

        figure.SetActive(false);
    }
}
