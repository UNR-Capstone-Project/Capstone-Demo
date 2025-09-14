using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "musicEvent", menuName = "Scriptable Objects/musicEvent")]
public class musicEvent : ScriptableObject
{
    [SerializeField] private float musicVolume = 1.0f;
    [SerializeField] private AudioSource[] musicLayers;
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    public float publicMusicVolume => musicVolume;
    public AudioSource[] publicMusicLayers => musicLayers;
    public AudioMixerGroup publicMusicMixerGroup => musicMixerGroup;
}
