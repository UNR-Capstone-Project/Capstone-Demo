using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SoundSystem;
public class musicPlayer : MonoBehaviour
{
    [SerializeField] private double startTime = AudioSettings.dspTime;
    [SerializeField] private float playerVolume = 1.0f;
    [SerializeField] private musicEvent song;
    [SerializeField] private List<AudioSource> songLayers = new List<AudioSource>();
    private Coroutine volumeFadingRoutine = null;
    public List<float> startingSongLayerVolumes = new List<float>();
    public musicEvent publicSong => song;

    public void setupSong(musicEvent givenSong)
    {
        //Debug.Log("setupSong called with: " + (givenSong == null ? "NULL" : givenSong.name));
        song = givenSong;
        setupLayers();
    }

    public void setupLayers()
    {
        songLayers.Clear();

        for (int i = 0; i < musicManager.maxLayerCount; i++)
        {
            songLayers.Add(gameObject.AddComponent<AudioSource>());
            Debug.Log("added Audio Source to a music player");
            songLayers[i].playOnAwake = false;
            songLayers[i].loop = true;

        }
    }

    public void setVolume(float newVolume)
    {
        //validation
        newVolume = Mathf.Clamp(newVolume, 0, 1);
        foreach (var layer in songLayers)
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

        AudioSource layer = songLayers[musicLayerIndex];
        layer.volume = newVolume;
        
    }

    public void play()
    {
        //Debug.Log("play() called with song: " + (song == null ? "NULL" : song.name));

        if (song == null)
        {
            Debug.Log("Song in the music player has a null musicEvent");
            return;
        }

        //iterate through existing layers with audio 
        for (int i = 0; i < song.publicMusicLayers.Length; i++)
        {
            Debug.Log(i);
            songLayers[i].clip = song.publicMusicLayers[i];
            songLayers[i].volume = song.publicMusicVolume;
            songLayers[i].loop = true;
            songLayers[i].PlayScheduled(startTime);
        }

    }

    public void stop()
    {
        if (song == null) return;

        foreach (var layer in songLayers)
        {
            if (layer == null) continue;

            layer.Stop();
        }
    }

    public void pause()
    {
        if (song == null) return;
        
        foreach (var layer in songLayers)
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

        foreach (var layer in songLayers)
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

        for (int i = 0; i < songLayers.Count; i++)
        {
            startingSongLayerVolumes.Add(songLayers[i].volume);
        }

        for (float elapsedTime = 0; elapsedTime < fadeTime; elapsedTime += Time.deltaTime)
        {
            //each frame, iterate through each layer and modulate its volume
            for (int i = 0; i < songLayers.Count; i++)
            {
                startVolume = startingSongLayerVolumes[i];
                newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                songLayers[i].volume = newVolume;
            }

            yield return null;
        }

        //ensuring accuracy of volume, boundary check for fadeTime
        for (int i = 0; i < songLayers.Count; i++)
        {
            songLayers[i].volume = targetVolume;
        }

    }

    
}
