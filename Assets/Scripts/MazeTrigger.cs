using UnityEngine;

public class MazeTrigger : MonoBehaviour
{
    public MazeRise maze;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // or whatever tag you're using
        {
            maze.TriggerRise();
        }
    }
}
