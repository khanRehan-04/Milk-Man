using UnityEngine;

public class MazeRise : MonoBehaviour
{
    public float riseHeight = 5f;
    public float riseSpeed = 2f;
    public ParticleSystem riseEffect;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isRising = false;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(0, riseHeight, 0);
        transform.position = startPosition;
    }

    void Update()
    {
        if (isRising)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isRising = false;
            }
        }
    }

    public void TriggerRise()
    {
        if (!isRising)
        {
            isRising = true;

            if (riseEffect != null)
            {
                riseEffect.Play();
                AudioManager.Instance.PlaySFX("rumble");
            }
        }
    }
}
