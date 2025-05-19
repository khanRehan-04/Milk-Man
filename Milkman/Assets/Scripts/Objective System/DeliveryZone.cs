using UnityEngine;
using System.Collections;

public class DeliveryZone : MonoBehaviour
{
    public GameObject popup;
    private float deliveryDelay = 1.5f;

    public void SetPopup(GameObject popupRef)
    {
        popup = popupRef;
        if (popup != null)
        {
            popup.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Popup reference not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MilkcanInteractable heldMilkcan = MilkcanManager.Instance.CurrentlyHeldCan;

            if (heldMilkcan != null)
            {
                heldMilkcan.Release();

                GameObject hand = GameObject.FindGameObjectWithTag("Hand");
                if (hand != null && hand.transform.childCount > 0)
                {
                    Transform milkCan = hand.transform.GetChild(0);
                    Destroy(milkCan.gameObject);
                }

                if (popup != null)
                {
                    popup.SetActive(true);
                }

                StartCoroutine(DeliverMilkAfterDelay());
            }
            else
            {
                Debug.Log("You need to be holding the milk can to deliver.");
            }
        }
    }

    private IEnumerator DeliverMilkAfterDelay()
    {
        yield return new WaitForSeconds(deliveryDelay);

        FindObjectOfType<ObjectiveManager>().DeliverMilk();

        if (popup != null)
        {
            popup.SetActive(false);
        }
    }
}
