using UnityEngine;

public class musicPlayer : MonoBehaviour
{
    [SerializeField] private double startTime = AudioSettings.dspTime;
    [SerializeField] private float playerVolume;
    [SerializeField] private musicEvent song;
    [SerializeField] private AudioSource[] songLayers;
    [SerializeField] private bool isPlaying;

    public musicEvent publicSong => song;

    private void Awake()
    {

    }

    void Start()
    {

    }

    public void play()
    {
        if (song == null)
        {
            return;
        }

        songLayers = song.publicMusicLayers;
        foreach (var layer in songLayers)
        {
            if (layer == null)
            {
                continue;
            }

            layer.outputAudioMixerGroup = song.publicMusicMixerGroup;
            layer.volume = song.publicMusicVolume;
            layer.PlayScheduled(startTime); //play all layers simultaneously
        }
    }

    public void stop()
    {
        if (song == null)
        {
            return;
        }

        songLayers = song.publicMusicLayers;
        foreach (var layer in songLayers)
        {
            if (layer == null)
            {
                continue;
            }

            layer.Stop();
        }
    }

    public void pause()
    {

    }

    public void fadeIn()
    {

    }

    public void fadeOut()
    {
        
    }
}
