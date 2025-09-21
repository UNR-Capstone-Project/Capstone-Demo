using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*

public class TempoManager : MonoBehaviour
{
    public const float TEMPO = 85.0f; //Beats per minute.
    public const float REACTION_INTERVAL = 0.25f; //How many seconds around the rhythm the player gets to react

    private const float RHYTHM = 60 / TEMPO;
    private bool onBeat = false;
    private double nextBeatTime;
    private InputAction noteHitAction;
    private MusicManager mMusicManager;

    void Start()
    {
        mMusicManager = GameObject.Find("ManagerObject").GetComponent<MusicManager>();
        mMusicManager.PlayMusicEvent += MusicStarted; //Subscribes to the event
    }

    void Awake()
    {
        noteHitAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        noteHitAction.started += CheckNoteHit;
        noteHitAction.Enable();
    }

    private void CheckNoteHit(InputAction.CallbackContext context)
    {
        if (onBeat)
        {
            Debug.Log("Key press was hit on beat!");
        }
    }

    void MusicStarted()
    {
        StopAllCoroutines();
        StartCoroutine(RhythmTimer());
    }

    IEnumerator RhythmTimer()
    {
        nextBeatTime = mMusicManager.getDspTime();

        while (true)
        {
            nextBeatTime += RHYTHM;
            double currentTime = mMusicManager.getDspTime();
            double waitTime = nextBeatTime - currentTime;

            if (waitTime > 0)
            {
                yield return new WaitForSeconds((float)waitTime);
            }
            else
            {
                yield return null;
            }

            onBeat = true;
            StartCoroutine(RhythmTimer());
            StartCoroutine(ReactionTimer());
        }
    }

    IEnumerator ReactionTimer()
    {
        yield return new WaitForSeconds(REACTION_INTERVAL);
        onBeat = false;
    }

    void OnDestroy()
    {
        mMusicManager.PlayMusicEvent -= MusicStarted; //Avoid memory leaks.
    }
}
*/