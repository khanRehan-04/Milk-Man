using UnityEngine;

public class HoldableObject : MonoBehaviour
{
    [Header("Layer Setup")]
    [SerializeField] private string heldLayer = "hand";
    [SerializeField] private string worldLayer = "Interactable";

    private bool isHeld;

    public void PickUp()
    {
        isHeld = true;
        SetLayerRecursively(gameObject, LayerMask.NameToLayer(heldLayer));
    }

    public void Drop()
    {
        isHeld = false;
        SetLayerRecursively(gameObject, LayerMask.NameToLayer(worldLayer));
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public bool IsHeld() => isHeld;
}
