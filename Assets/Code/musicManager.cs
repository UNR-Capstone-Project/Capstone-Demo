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

                if (_instance == null)
                {
                    _instance = CreateInstance<musicManager>();
                }
            }

            return _instance;
        }
    }

    [SerializeField] private float globalMusicVolume;
    [SerializeField] private List<musicPlayer> activePlayers;
    [SerializeField] private List<musicPlayer> inactivePlayers; 
    [SerializeField] private musicPlayer currentPlayer;
    [SerializeField] private float crossfadeTime = 0.5f;

    public void playSong(musicEvent s)
    {
        //how should it interact with active and inactive player list

        foreach (musicPlayer Player in activePlayers)
        {

        }
    }

    public void playSong(GameObject prefab)
    {
        var player = Instantiate(prefab);
        currentPlayer = player.GetComponent<musicPlayer>();

        if (currentPlayer == null) return;

        currentPlayer.play();
    }

    public void playSong(musicPlayer p)
    {
        p.play();
    }

    public void stopSong(musicEvent s)
    {
        //how should it interact with active and inactive player list

        foreach (musicPlayer Player in activePlayers)
        {

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
