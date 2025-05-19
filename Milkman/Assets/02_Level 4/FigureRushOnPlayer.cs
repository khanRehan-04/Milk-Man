using System.Collections;
using UnityEngine;

public class FigureRushOnPlayer : MonoBehaviour
{
    public GameObject figure;
    public Transform appearPosition;
    public float waitBeforeRush = 1.5f;
    public float rushSpeed = 10f;
    public float rushDuration = 1f;

    private bool triggered = false;
    private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || other.tag != "Player") return;

        triggered = true;
        player = other.transform;
        StartCoroutine(RushAtPlayer());
    }

    IEnumerator RushAtPlayer()
    {
        figure.transform.position = appearPosition.position;
        figure.SetActive(true);

        yield return new WaitForSeconds(waitBeforeRush);

        Vector3 direction = (player.position - figure.transform.position).normalized;
        AudioManager.Instance.PlaySFX("late");

        float timer = 0f;
        while (timer < rushDuration)
        {
            figure.transform.position += direction * rushSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        figure.SetActive(false);
    }
}
