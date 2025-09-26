using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

[CreateAssetMenu(fileName = "musicManager", menuName = "Scriptable Objects/musicManager")]
public class musicManager : ScriptableObject
{
    private GameObject musicPlayerPrefab;
    public const int maxLayerCount = 5;
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

    //unsure if these list should instead be queues of some kind, depends on how many players we want in a given scene/level at a time
    //unsure of memory implications 
    [SerializeField] private List<musicPlayer> inactivePlayers = new List<musicPlayer>();
    [SerializeField] private musicPlayer currentPlayer;
    [SerializeField] private float crossfadeTime = 0.5f;

    private void Awake()
    {
        //attach prefab 
        musicPlayerPrefab = Resources.Load<GameObject>("musicPlayer");
    }

    public void playSong(musicEvent s)
    {
        if (currentPlayer == null && inactivePlayers.Count == 0)
        {
            //create a player 
            GameObject newObject = Instantiate(musicPlayerPrefab);
            currentPlayer = newObject.GetComponent<musicPlayer>();
            currentPlayer.setupSong(s);

            //play the music event passed in 
            currentPlayer.play(s);
        }
        else
        {
            foreach (musicPlayer Player in inactivePlayers)
            {
                if (Player.publicSong == s)
                {

                }
            }
        }

    }

    public void stopSong(musicEvent s)
    {
        
    }

    public void pauseCurrentSong()
    {
        if (currentPlayer == null)
        {
            
        }
    }

    public void unpauseCurrentSong()
    {
        if (currentPlayer == null)
        {
            
        }
    }
}
