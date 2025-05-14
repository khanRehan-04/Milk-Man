using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public TimelineEventHandler cutsceneHandler; // Reference to the TimelineEventHandler
    private bool cutscenePlaying = false; // Prevents re-triggering while playing

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cutscenePlaying)
        {
            cutscenePlaying = true;
            cutsceneHandler.PlayNextCutscene();
        }
    }

    private void OnEnable()
    {
        // Subscribe to cutscene end event
        if (cutsceneHandler != null)
        {
            cutsceneHandler.playableDirector.stopped += OnCutsceneEnd;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (cutsceneHandler != null)
        {
            cutsceneHandler.playableDirector.stopped -= OnCutsceneEnd;
        }
    }

    private void OnCutsceneEnd(UnityEngine.Playables.PlayableDirector director)
    {
        cutscenePlaying = false; // Allow triggering again when cutscene ends
    }
}
