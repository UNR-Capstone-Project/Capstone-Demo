using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[CreateAssetMenu(fileName = "musicManager", menuName = "Scriptable Objects/musicManager")]
public class musicManager : ScriptableObject
{
    private static musicManager _instance;
    public static musicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<musicManager>("MusicManager");
            }

            return _instance;
        }
    }

    [SerializeField] private float globalMusicVolume;
    [SerializeField] private List<musicPlayer> activePlayers;
    [SerializeField] private musicPlayer currPlayer;
    [SerializeField] private float crossfadeTime = 0.5f;


    public void debugMessage()
    {
        Debug.Log("Hello, this manager exists in the scene!");
    }

    public void playSong(musicEvent s)
    {
        foreach (musicPlayer Player in activePlayers)
        {
            if (Player.publicSong == s)
            {
                Player.play();
            }
        }
    }

    public void playSong(musicPlayer p)
    {
        p.play();
    }

    public void stopSong(musicEvent s)
    {
        foreach (musicPlayer Player in activePlayers)
        {
            if (Player.publicSong == s)
            {
                Player.stop();
            }
        }
    }

    public void stopSong(musicPlayer p)
    {
        p.stop();
    }

    public void stopAllSongs()
    {
        foreach (musicPlayer Player in activePlayers)
        {
            Player.stop();
        }
    }
    
    
}
