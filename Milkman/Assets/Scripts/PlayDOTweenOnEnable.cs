using UnityEngine;
using DG.Tweening;

public class PlayDOTweenOnEnable : MonoBehaviour
{
    [SerializeField] private float popupDuration = 2f;

    void OnEnable()
    {
        var anims = GetComponents<DOTweenAnimation>();
        foreach (var anim in anims)
        {
            anim.DORewind();
            anim.DOPlay();
        }

        AudioManager.Instance.PlaySFX("popup");

        // Auto-disable popup after duration
        Invoke(nameof(DisablePopup), popupDuration + 1f);
    }

    private void DisablePopup()
    {
        gameObject.SetActive(false);
    }
}