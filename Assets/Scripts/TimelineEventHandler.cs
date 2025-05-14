using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.Timeline;

public class TimelineEventHandler : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneData
    {
        public TimelineAsset timelineAsset; // The Timeline asset for this cutscene
        public GameObject[] activateDuringCutscene;   // Objects activated when cutscene starts, deactivated when it ends
        public GameObject[] deactivateDuringCutscene; // Objects deactivated when cutscene starts, activated when it ends
    }

    public PlayableDirector playableDirector; // Single PlayableDirector in scene
    public CutsceneData[] cutscenes; // Array to store multiple cutscenes

    private int currentCutsceneIndex = 0;

    private void Start()
    {
        if (playableDirector == null)
        {
            Debug.LogError("PlayableDirector not assigned!");
            return;
        }

        if (cutscenes.Length > 0)
        {
            PlayCutscene(0);
        }
        else
        {
            Debug.LogError("No cutscenes assigned in TimelineEventHandler!");
        }
    }

    public void PlayNextCutscene()
    {
        if (currentCutsceneIndex + 1 < cutscenes.Length)
        {
            PlayCutscene(++currentCutsceneIndex);
        }
        else
        {
            Debug.Log("No more cutscenes to play.");
        }
    }

    public void PlayCutscene(int index)
    {
        if (index < 0 || index >= cutscenes.Length) return;

        CutsceneData cutscene = cutscenes[index];

        if (cutscene.timelineAsset == null)
        {
            Debug.LogError("Timeline asset not assigned for cutscene at index " + index);
            return;
        }

        // Activate/deactivate objects at cutscene start
        foreach (GameObject obj in cutscene.activateDuringCutscene)
        {
            if (obj) obj.SetActive(true);
        }

        foreach (GameObject obj in cutscene.deactivateDuringCutscene)
        {
            if (obj) obj.SetActive(false);
        }

        // Assign Timeline asset to the PlayableDirector and play it
        playableDirector.playableAsset = cutscene.timelineAsset;
        playableDirector.stopped += OnTimelineEnd;
        playableDirector.Play();
    }

    private void OnTimelineEnd(PlayableDirector director)
    {
        if (director != playableDirector) return;

        CutsceneData cutscene = cutscenes[currentCutsceneIndex];

        // Revert activation states after cutscene ends
        foreach (GameObject obj in cutscene.activateDuringCutscene)
        {
            if (obj) obj.SetActive(false);
        }

        foreach (GameObject obj in cutscene.deactivateDuringCutscene)
        {
            if (obj) obj.SetActive(true);
        }

        // If this was the last cutscene, mark the city as completed and reload the game
        if (currentCutsceneIndex == cutscenes.Length - 1)
        {
            StartCoroutine(HandleEndGame());
        }

        // Unsubscribe to prevent memory leaks
        playableDirector.stopped -= OnTimelineEnd;
    }

    private IEnumerator HandleEndGame()
    {
        GameManager.Instance.CompleteCity(); // Mark the city as completed
        yield return null; // Wait for a frame
        GameManager.Instance.LoadGameplay(); // Load the next scene
    }
}
