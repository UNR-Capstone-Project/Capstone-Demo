using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: Add collision checks for notes.
//TODO: Get user input if note is colliding with the current time.
//TODO: Get percentage of time that note was held, then deal accordingly.
//TODO: Despawn notes that are no longer in range of the time frame.
//TODO: Determine how many notes player needs to consecutively hit to perform attack.
//TODO: Adjust engine timescale outside of the scope of this function.

public class MusicManager : MonoBehaviour
{
    public const float timeWindow = 6f; //How many seconds are centering the surrounding the current track time of the song that are previewed within the minigame. Essentially your "view" of the current song.
    public static float reactionTime = timeWindow / 2; //Time in seconds that a note will spawn before reaching its onset time.

    private const float tempo = 60f; //Tempo is measured in beats per minute
    private static float beat = 60f / tempo;
    
    private float currentTrackTime = 0f;
    private int noteCount = 0;

    private note[] song;

    private enum NOTE_PITCH
    {
        A = 0,
        ASharp = 1,
        BFlat = 1,
        B = 2,
        C = 3,
        CSharp = 4,
        DFlat = 4,
        D = 5,
        DSharp = 6,
        EFlat = 6,
        E = 7,
        F = 8,
        FSharp = 9,
        GFlat = 9,
        G = 10,
        GSharp = 11,
        AFlat = 11
    }

    private enum NOTE_NAME
    {
        WholeNote,
        HalfNote,
        QuarterNote,
        EighthNote
    }

    private static Dictionary<NOTE_NAME, float> noteDuration = new Dictionary<NOTE_NAME, float>
    { //How many beats a note is.
        {NOTE_NAME.WholeNote, 4.0f },
        {NOTE_NAME.HalfNote, 2.0f },
        {NOTE_NAME.QuarterNote, 1.0f },
        {NOTE_NAME.EighthNote, 0.5f }
    };

    private struct note
    {
        public NOTE_PITCH pitch;
        public float duration; //Note duration in seconds
        public float onset;

        public note(NOTE_NAME name, NOTE_PITCH pitch, float onset)
        {

            this.duration = beat * noteDuration[name];
            this.pitch = pitch;
            this.onset = onset;
        }
    }
    
    void Start()
    {
        song = new note[3]; //A song with 3 total notes.
        song[0] = new note (NOTE_NAME.QuarterNote, NOTE_PITCH.A, 1.26f);
    }
    
    void Update()
    {
        currentTrackTime += Time.deltaTime;
        //ISSUE: To sync with audio use audioSource.time; instead!

        int noteCachedAmount = 4; //How many notes can be spawned simultaneously.
        int maxCachedAmount = Mathf.Min(noteCount + noteCachedAmount, song.Length);

        for (int i = noteCount; i < maxCachedAmount; i++)
        { //Cache reduces looping for songs with many notes.
            float spawnNoteTime = song[i].onset - reactionTime;
            float tolerance = 0.01f;

            if (currentTrackTime >= spawnNoteTime && currentTrackTime < spawnNoteTime + tolerance)
            {
                SpawnNote(song[i]);
                noteCount++;
            }
        }
    }

    void SpawnNote(note currentNote) //ISSUE: Probably better to use a coroutine to handle the animation!
    {
        //Spawn note here!
        //Note must positionally reach the end of the visible window in "timeWindow" seconds.
        //Basically Lerp the notes position by a linear factor to the end.
    }
}
