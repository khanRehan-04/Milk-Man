using DG.Tweening;
using UnityEngine;

public class PlayDOTweenOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        var anims = GetComponents<DOTweenAnimation>();
        foreach (var anim in anims)
        {
            anim.DORewind(); // Reset animation so it plays from beginning
            anim.DOPlay();   // Play animation
        }

        AudioManager.Instance.PlaySFX("popup");
    }
}
