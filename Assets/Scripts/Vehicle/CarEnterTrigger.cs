using UnityEngine;
using UnityEngine.UI;

public class CarEnterTrigger : MonoBehaviour
{
    public GameObject enterCarButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterCarButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enterCarButton.SetActive(false);
        }
    }
}
