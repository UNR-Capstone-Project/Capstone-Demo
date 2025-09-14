using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class musicPlayer : MonoBehaviour
{
    [SerializeField] private double startTime = AudioSettings.dspTime;
    [SerializeField] private float playerVolume;
    [SerializeField] private musicEvent song;
    //[SerializeField] public musicEvent currentSong; 
    //ex: 0 = ambient, 1 = tension, 2 = moderate tension, 3 = high tension, 4 = boss fight 
    //TODO: can change over to enums later probably
    [SerializeField] private int prioritySongLayer = 0;
    [SerializeField] private AudioSource[] songLayers;
    private Coroutine volumeFadingRoutine = null;
    public List<float> startingSongLayerVolumes = new List<float>();

    public musicEvent publicSong => song;
    public AudioSource[] publicSongLayers => songLayers;

    public void setVolume(float newVolume)
    {
        //validation
        newVolume = Mathf.Clamp(newVolume, 0, 1);
        foreach (var layer in song.publicMusicLayers)
        {
            if (layer == null) continue;

            layer.volume = newVolume;
        }
        playerVolume = newVolume;
    }

    public void setVolume(int musicLayerIndex, float newVolume)
    {
        //validation
        newVolume = Mathf.Clamp(newVolume, 0, 1);
        if (musicLayerIndex < 0 || musicLayerIndex > song.publicMusicLayers.Length)
        {
            Debug.Log("Music Layer selected for changing volume exceeds layers available or is a negative value");
            return;
        }

        AudioSource layer = song.publicMusicLayers[musicLayerIndex];
        layer.volume = newVolume;
        
    }

    public void play()
    {
        if (song == null) return;

        foreach (var layer in song.publicMusicLayers)
        {
            if (layer == null) continue;

            layer.outputAudioMixerGroup = song.publicMusicMixerGroup;
            layer.volume = song.publicMusicVolume;
            layer.loop = false;
            layer.PlayScheduled(startTime); //play all layers simultaneously
        }
    }

    public void stop()
    {
        if (song == null) return;

        foreach (var layer in song.publicMusicLayers)
        {
            if (layer == null) continue;

            layer.Stop();
        }
    }

    public void pause()
    {
        if (song == null) return;
        
        foreach (var layer in song.publicMusicLayers)
        {
            if (layer.isPlaying)
            {
                layer.Pause();
            }
        }
    }

    public void unpause()
    {
        if (song == null) return;

        foreach (var layer in song.publicMusicLayers)
        {
            if (!layer.isPlaying)
            {
                layer.UnPause();
            }
        }
    }

    public void fadeVolume(float destinationVolume, float fadeTime)
    {
        //validation 
        if (fadeTime < 0)
        {
            fadeTime = 0.5f;
        }

        //kill coroutine if one is currently going, prevent players from animating same song at same time 
        if (volumeFadingRoutine != null)
        {
            StopCoroutine(volumeFadingRoutine);
        }

        volumeFadingRoutine = StartCoroutine(fadeVolumeCoroutine(destinationVolume, fadeTime));
    }

    public IEnumerator fadeVolumeCoroutine(float targetVolume, float fadeTime)
    {
        float startVolume;
        float newVolume;

        //clear current volume list and attach new volumes on start of coroutine
        startingSongLayerVolumes.Clear();

        for (int i = 0; i < publicSongLayers.Length; i++)
        {
            startingSongLayerVolumes.Add(publicSongLayers[i].volume);
        }

        for (float elapsedTime = 0; elapsedTime < fadeTime; elapsedTime += Time.deltaTime)
        {
            //each frame, iterate through each layer and modulate its volume
            for (int i = 0; i < publicSongLayers.Length; i++)
            {
                startVolume = startingSongLayerVolumes[i];
                newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                song.publicMusicLayers[i].volume = newVolume;
            }

            yield return null;
        }

        //ensuring accuracy of volume, boundary check for fadeTime
        for (int i = 0; i < publicSongLayers.Length; i++)
        {
            song.publicMusicLayers[i].volume = targetVolume;
        }

    }

    
}
