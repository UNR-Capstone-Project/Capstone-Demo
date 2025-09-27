using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    [CreateAssetMenu(fileName = "musicEvent", menuName = "Scriptable Objects/musicEvent")]
    public class musicEvent : ScriptableObject
    {
        [SerializeField] float musicVolume = 1.0f;
        [SerializeField] AudioClip[] musicLayers;
        [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] int musicTempo;

        public float publicMusicVolume => musicVolume;
        public AudioClip[] publicMusicLayers => musicLayers;
        public AudioMixerGroup publicMusicMixerGroup => musicMixerGroup;

        /*public void Play()
        {
            musicManager.Instance.playSong(this);
        }*/
    }
}


