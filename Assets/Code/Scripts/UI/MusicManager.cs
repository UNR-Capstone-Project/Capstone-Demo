using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour
{
    private const float tempo = 60f; //Tempo is measured in beats per minute
    private static float beat = 60f / tempo;
    private float songLength = 60f; //Measured in seconds

    private enum noteName
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

    private static Dictionary<string, float> noteDuration = new Dictionary<string, float>
    {
        {"WholeNote", 4.0f },
        {"HalfNote", 2.0f },
        {"QuarterNote", 1.0f },
        {"EighthNote", 0.5f }
    };

    private struct note
    {
        public noteName pitch;
        public float duration; //Note duration in seconds
        public float onset;

        public note(string durationName, noteName pitch, float onset)
        {

            this.duration = beat * noteDuration[durationName];
            this.pitch = pitch;
            this.onset = onset;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
